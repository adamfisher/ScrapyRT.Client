using System;
using Newtonsoft.Json;

namespace ScarpyRT.Client
{
    public class CrawlStatistics
    {
        [JsonProperty("start_time")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("finish_time")]
        public DateTimeOffset FinishTime { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }

        [JsonProperty("downloader/response_status_count/200")]
        public long DownloaderResponseStatusCount200 { get; set; }

        [JsonProperty("downloader/response_count")]
        public long DownloaderResponseCount { get; set; }

        [JsonProperty("downloader/response_bytes")]
        public long DownloaderResponseBytes { get; set; }

        [JsonProperty("downloader/request_method_count/GET")]
        public long DownloaderRequestMethodCountGet { get; set; }

        [JsonProperty("downloader/request_count")]
        public long DownloaderRequestCount { get; set; }

        [JsonProperty("downloader/request_bytes")]
        public long DownloaderRequestBytes { get; set; }

        [JsonProperty("item_scraped_count")]
        public long ItemScrapedCount { get; set; }

        [JsonProperty("log_count/DEBUG")]
        public long LogCountDebug { get; set; }

        [JsonProperty("log_count/INFO")]
        public long LogCountInfo { get; set; }

        [JsonProperty("response_received_count")]
        public long ResponseReceivedCount { get; set; }

        [JsonProperty("scheduler/dequeued")]
        public long SchedulerDequeued { get; set; }

        [JsonProperty("scheduler/dequeued/memory")]
        public long SchedulerDequeuedMemory { get; set; }

        [JsonProperty("scheduler/enqueued")]
        public long SchedulerEnqueued { get; set; }

        [JsonProperty("scheduler/enqueued/memory")]
        public long SchedulerEnqueuedMemory { get; set; }
    }
}