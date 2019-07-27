using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ScarpyRT.Client
{
    /// <summary>
    /// Client providing access to a ScrapyRT server instance.
    /// </summary>
    public sealed class ScrapyRTClient
    {
        #region Properties

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        private HttpClient Client { get; }

        /// <summary>
        /// Gets the json serializer settings.
        /// </summary>
        /// <value>
        /// The json serializer settings.
        /// </value>
        private JsonSerializerSettings SerializerSettings { get; }

        /// <summary>
        /// Gets the resource URI.
        /// </summary>
        /// <value>
        /// The resource URI.
        /// </value>
        private Uri ResourceUri { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapyRTClient" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ScrapyRTClient(HttpClient client)
        {
            Client = client;
            ResourceUri = new Uri(Client.BaseAddress, "crawl.json");
            SerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapyRTClient" /> class.
        /// </summary>
        /// <param name="serverAddress">The server address of the scrapyrt web server.</param>
        public ScrapyRTClient(string serverAddress) : this(new HttpClient() {BaseAddress = new Uri(serverAddress)})
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs a GET request to retrieve the first scraped item returned from the spider, null otherwise.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="spiderName">Name of the spider.</param>
        /// <param name="url">The URL.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="maxRequests">The maximum requests.</param>
        /// <param name="startRequests">The start requests.</param>
        /// <returns></returns>
        public Task<TItem> GetSpiderSingleItemAsync<TItem>(
            string spiderName,
            string url,
            string callback = null,
            int? maxRequests = null,
            bool? startRequests = null) =>
            GetSpiderItemsAsync<TItem>(spiderName, url, callback, maxRequests, startRequests)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result.FirstOrDefault() : default);

        /// <summary>
        /// Performs a GET request to retrieve the list of scraped items returned from the spider.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="spiderName">Name of the spider.</param>
        /// <param name="url">The URL.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="maxRequests">The maximum requests.</param>
        /// <param name="startRequests">The start requests.</param>
        /// <returns></returns>
        public Task<List<TItem>> GetSpiderItemsAsync<TItem>(
            string spiderName,
            string url,
            string callback = null,
            int? maxRequests = null,
            bool? startRequests = null) =>
            GetSpiderCrawlAsync<TItem>(spiderName, url, callback, maxRequests, startRequests)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result?.Items : default);

        /// <summary>
        /// Performs a GET request to retrieve the spider crawl response.
        /// If required parameters are missing api will return 400 Bad Request with hopefully helpful error message.
        /// </summary>
        /// <typeparam name="TItem">The type of the scraped item.</typeparam>
        /// <param name="spiderName">Name of the spider to be scheduled. If spider is not found api will return 404.</param>
        /// <param name="url">
        /// Absolute URL to send request to.
        /// By default API will crawl this url and won’t execute any other requests.
        /// Most importantly it will not execute <c>start_requests</c> and spider will not visit urls defined in start_urls spider attribute.
        /// There will be only one single request scheduled in API - request for resource identified by url argument.
        /// If you want to execute request pass <c>start_requests</c> argument.
        /// </param>
        /// <param name="callback">
        /// Should exist as method of scheduled spider, does not need to contain self.
        /// If not passed or not found on spider default callback parse will be used.
        /// </param>
        /// <param name="maxRequests">
        /// Maximum amount of requests spider can generate. E.g. if it is set to 1 spider will only schedule one single request,
        /// other requests generated by spider (for example in callback, following links in first response) will be ignored.
        /// If your spider generates many requests in callback and you don’t want to wait forever
        /// for it to finish you should probably pass it.
        /// </param>
        /// <param name="startRequests">
        /// Whether spider should execute Scrapy.Spider.start_requests method. start_requests are executed by default
        /// when you run Scrapy Spider normally without ScrapyRT, but this method is NOT executed in API by default.
        /// By default we assume that spider is expected to crawl ONLY url provided in parameters without making any
        /// requests to start_urls defined in Spider class. start_requests argument overrides this behavior.
        /// If this argument is present API will execute start_requests Spider method.
        /// </param>
        /// <returns>The response from the crawl.</returns>
        public Task<CrawlResponse<TItem>> GetSpiderCrawlAsync<TItem>(
            string spiderName, 
            string url, 
            string callback = null,
            int? maxRequests = null,
            bool? startRequests = null)
        {
            var uriBuilder = new UriBuilder(ResourceUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["spider_name"] = spiderName;

            if (url != null)
                query["url"] = HttpUtility.UrlEncode(url);

            if (callback != null)
                query["callback"] = callback;

            if (maxRequests != null)
                query["max_requests"] = maxRequests.ToString();
            
            if (startRequests != null)
                query["start_requests"] = startRequests.ToString();

            uriBuilder.Query = query.ToString();
            var scrapyUrl = uriBuilder.ToString();

            return Client.GetAsync(scrapyUrl)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result?.EnsureSuccessStatusCode()?.Content?.ReadAsStringAsync() : null)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result?.Result : null)
                .ContinueWith(task => JsonConvert.DeserializeObject<CrawlResponse<TItem>>(task.Result, SerializerSettings));
        }

        /// <summary>
        /// Performs a POST request to retrieve the first scraped item returned from the spider, null otherwise.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="crawlRequest">The crawl request.</param>
        /// <returns></returns>
        public Task<TItem> PostSpiderSingleItemAsync<TItem>(CrawlRequest crawlRequest) =>
            PostSpiderItemsAsync<TItem>(crawlRequest)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result.FirstOrDefault() : default);

        /// <summary>
        /// Performs a POST request to retrieve the list of scraped items returned from the spider.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="crawlRequest">The crawl request.</param>
        /// <returns></returns>
        public Task<List<TItem>> PostSpiderItemsAsync<TItem>(CrawlRequest crawlRequest) =>
            PostSpiderCrawlAsync<TItem>(crawlRequest)
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result.Items : default);

        /// <summary>
        /// Performs a POST request to retrieve the spider crawl response.
        /// </summary>
        /// <typeparam name="TItem">The type of the scraped item.</typeparam>
        /// <param name="crawlRequest">The request.</param>
        /// <returns></returns>
        public Task<CrawlResponse<TItem>> PostSpiderCrawlAsync<TItem>(CrawlRequest crawlRequest) =>
            Client.PostAsync(ResourceUri, new StringContent(JsonConvert.SerializeObject(crawlRequest, SerializerSettings)))
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result.EnsureSuccessStatusCode()?.Content?.ReadAsStringAsync() : null )
                .ContinueWith(task => task.Status == TaskStatus.RanToCompletion ? task.Result?.Result : null)
                .ContinueWith(task => JsonConvert.DeserializeObject<CrawlResponse<TItem>>(task.Result, SerializerSettings));

        #endregion
    }
}
