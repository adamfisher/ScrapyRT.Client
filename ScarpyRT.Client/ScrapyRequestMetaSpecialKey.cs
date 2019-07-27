namespace ScarpyRT.Client
{
    /// <summary>
    /// The Request.meta attribute can contain any arbitrary data, but there are some special keys recognized by Scrapy and its built-in extensions.
    /// </summary>
    public class ScrapyRequestMetaSpecialKey
    {
        public static readonly string DontRedirect = "dont_redirect";
        public static readonly string DontRetry = "dont_retry";
        public static readonly string HandleHttpstatusList = "handle_httpstatus_list";
        public static readonly string HandleHttpstatusAll = "handle_httpstatus_all";
        public static readonly string DontMergeCookies = "dont_merge_cookies";
        public static readonly string Cookiejar = "cookiejar";
        public static readonly string DontCache = "dont_cache";
        public static readonly string RedirectReasons = "redirect_reasons";
        public static readonly string RedirectUrls = "redirect_urls";
        public static readonly string Bindaddress = "bindaddress";
        public static readonly string DontObeyRobotstxt = "dont_obey_robotstxt";
        public static readonly string DownloadTimeout = "download_timeout";
        public static readonly string DownloadMaxsize = "download_maxsize";
        public static readonly string DownloadLatency = "download_latency";
        public static readonly string DownloadFailOnDataloss = "download_fail_on_dataloss";
        public static readonly string Proxy = "proxy";
        public static readonly string FtpUser = "ftp_user";
        public static readonly string FtpPassword = "ftp_password";
        public static readonly string ReferrerPolicy = "referrer_policy";
        public static readonly string MaxRetryTimes = "max_retry_times";
    }
}
