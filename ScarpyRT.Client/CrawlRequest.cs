using Newtonsoft.Json;

namespace ScarpyRT.Client
{
    /// <summary>
    /// Contains information about request to be scheduled with spider and spider name.
    /// All positional and keyword arguments for Scrapy Request should be placed in request JSON key.
    /// </summary>
    public class CrawlRequest
    {
        /// <summary>
        /// Name of the spider to be scheduled. If spider is not found api will return 404.
        /// </summary>
        /// <value>
        /// The name of the spider. This property is required.
        /// </value>
        [JsonProperty("spider_name", Required = Required.Always)]
        public string SpiderName { get; set; }

        /// <summary>
        /// Maximal amount of requests spider can generate.
        /// </summary>
        /// <value>
        /// The maximum requests. This property is optional.
        /// </value>
        [JsonProperty("max_requests")]
        public int MaxRequests { get; set; }

        /// <summary>
        /// Arguments passed to the Scrapy request object that will be created and scheduled with the spider.
        /// The Url property is required.
        /// </summary>
        /// <value>
        /// The request. This property is required.
        /// </value>
        [JsonProperty("request", Required = Required.Always)]
        public TwistedRequest Request { get; set; }
    }
}
