using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScarpyRT.Client
{
    public class CrawlResponse<TItem>
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("spider_name")]
        public string SpiderName { get; set; }

        [JsonProperty("stats")]
        public CrawlStatistics CrawlStatistics { get; set; }

        [JsonProperty("items")]
        public List<TItem> Items { get; set; }

        [JsonProperty("items_dropped")]
        public List<TItem> ItemsDropped { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}
