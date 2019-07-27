using FluentAssertions;
using Newtonsoft.Json;
using ScarpyRT.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace ScrapyRT.Client.Tests
{
    public class ScrapyRTClientTests
    {
        #region Properties

        public string PostCrawlRequestString { get; set; }
        public string CrawlResponseString { get; set; }
        public JsonSerializerSettings JsonSettings { get; set; }

        #endregion

        #region Constructors

        public ScrapyRTClientTests()
        {
            PostCrawlRequestString = 
@"{
    ""request"": {
        ""url"": ""http://www.target.com/p/-/A-13631176"",
        ""meta"": {
            ""category"": ""apparel"",
            ""item"": {
                ""discovery_item_id"": ""999""
            }
        },
        ""callback"": ""parse_product"",
        ""dont_filter"": ""True"",
        ""cookies"": {
            ""foo"": ""bar""
        }
    },
    ""spider_name"": ""target.com_products""
}";

            CrawlResponseString =
@"{
    ""status"": ""ok"",
    ""spider_name"": ""target.com_products"",
    ""stats"": {
        ""start_time"": ""2014-12-29 16:04:15"",
        ""finish_time"": ""2014-12-29 16:04:16"",
        ""finish_reason"": ""finished"",
        ""downloader/response_status_count/200"": 1,
        ""downloader/response_count"": 1,
        ""downloader/response_bytes"": 8494,
        ""downloader/request_method_count/GET"": 1,
        ""downloader/request_count"": 1,
        ""downloader/request_bytes"": 247,
        ""item_scraped_count"": 16,
        ""log_count/DEBUG"": 17,
        ""log_count/INFO"": 4,
        ""response_received_count"": 1,
        ""scheduler/dequeued"": 1,
        ""scheduler/dequeued/memory"": 1,
        ""scheduler/enqueued"": 1,
        ""scheduler/enqueued/memory"": 1
    },
    ""items"": [
        {
            ""description"": ""Protects you in the winter!"",
            ""name"": ""Scarf"",
            ""url"": ""http://target.com/scarf""
        },
        {
            ""description"": ""Cools you off in the summer!"",
            ""name"": ""Swimming Trunks"",
            ""url"": ""http://target.com/swimming-trunks""
        }
    ],
    ""items_dropped"": []
}";

            JsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        #endregion

        #region Methods

        #region GET Methods

        [Fact]
        public async void GetSpiderSingleItemAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var expectedResponse = GetCrawlResponseObject<TargetComItem>().Items.First();
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.GetSpiderSingleItemAsync<TargetComItem>("SomeSpider", "https://google.com");

                response.Should()
                    .BeOfType<TargetComItem>()
                    .And
                    .BeEquivalentTo(expectedResponse);
            }
        }

        [Fact]
        public async void GetSpiderItemsAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var expectedResponse = GetCrawlResponseObject<TargetComItem>().Items;
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.GetSpiderItemsAsync<TargetComItem>("SomeSpider", "https://google.com");

                response.Should()
                    .BeOfType<List<TargetComItem>>()
                    .And
                    .BeEquivalentTo(expectedResponse)
                    .And
                    .HaveCount(2);
            }
        }

        [Fact]
        public async void GetSpiderCrawlAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var expectedResponse = GetCrawlResponseObject<TargetComItem>();
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.GetSpiderCrawlAsync<TargetComItem>("SomeSpider", "https://google.com");
                
                response.Should()
                    .BeOfType<CrawlResponse<TargetComItem>>()
                    .And
                    .BeEquivalentTo(expectedResponse);

                response.Items.Should()
                    .HaveCount(2)
                    .And
                    .BeEquivalentTo(expectedResponse.Items);
            }
        }

        [Fact]
        public async void GetSpiderCrawlAsync_ValidUrl()
        {
            using(var httpMessageHandler = new FakeHttpMessageHandler<string>(CrawlResponseString))
            using(var httpClient = new HttpClient(httpMessageHandler) {BaseAddress = new Uri("https://localhost:9080")})
            {
                var scrapyClient = new ScrapyRTClient(httpClient);

                await scrapyClient.GetSpiderCrawlAsync<TargetComItem>(
                    "SomeSpider", 
                    "https://google.com",
                    "callback_method",
                    14,
                    true);

                httpMessageHandler.ResponseUri.ToString().Should()
                    .BeEquivalentTo("https://localhost:9080/crawl.json?spider_name=SomeSpider&url=https%253a%252f%252fgoogle.com&callback=callback_method&max_requests=14&start_requests=True");
            }
        }

        #endregion

        #region POST Methods

        [Fact]
        public async void PostSpiderSingleItemAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var request = GetCrawlRequestObject();
                var expectedResponse = GetCrawlResponseObject<TargetComItem>().Items.First();
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.PostSpiderSingleItemAsync<TargetComItem>(request);

                response.Should()
                    .BeOfType<TargetComItem>()
                    .And
                    .BeEquivalentTo(expectedResponse);
            }
        }

        [Fact]
        public async void PostSpiderItemsAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var request = GetCrawlRequestObject();
                var expectedResponse = GetCrawlResponseObject<TargetComItem>().Items;
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.PostSpiderItemsAsync<TargetComItem>(request);

                response.Should()
                    .BeOfType<List<TargetComItem>>()
                    .And
                    .BeEquivalentTo(expectedResponse)
                    .And
                    .HaveCount(2);
            }
        }

        [Fact]
        public async void PostSpiderCrawlAsync()
        {
            using (var httpClient = GetFakeHttpClient(CrawlResponseString))
            {
                var request = GetCrawlRequestObject();
                var expectedResponse = GetCrawlResponseObject<TargetComItem>();
                var scrapyClient = new ScrapyRTClient(httpClient);

                var response = await scrapyClient.PostSpiderCrawlAsync<TargetComItem>(request);

                response.Should()
                    .BeOfType<CrawlResponse<TargetComItem>>()
                    .And
                    .BeEquivalentTo(expectedResponse);

                response.Items.Should()
                    .HaveCount(2)
                    .And
                    .BeEquivalentTo(expectedResponse.Items);
            }
        }

        #endregion

        private HttpClient GetFakeHttpClient(string jsonResponse)
        {
            var httpMessageHandler = new FakeHttpMessageHandler<string>(jsonResponse);
            var httpClient = new HttpClient(httpMessageHandler) {BaseAddress = new Uri("https://localhost:9080")};
            return httpClient;
        }

        private CrawlRequest GetCrawlRequestObject() => JsonConvert.DeserializeObject<CrawlRequest>(PostCrawlRequestString, JsonSettings);

        private CrawlResponse<TItem> GetCrawlResponseObject<TItem>() => 
            JsonConvert.DeserializeObject<CrawlResponse<TItem>>(CrawlResponseString, JsonSettings);

        #endregion
    }
}
