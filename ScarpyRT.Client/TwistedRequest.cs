using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace ScarpyRT.Client
{
    public class TwistedRequest
    {
        /// <summary>
        /// A string containing the URL of this request. Keep in mind that this
        /// attribute contains the escaped URL, so it can differ from the URL
        /// passed in the constructor.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [JsonProperty("url", Required = Required.Always)]
        public Uri Url { get; set; }

        /// <summary>
        /// The function that will be called with the response of this request (once its downloaded) as its first parameter.
        /// If a Request does not specify a callback, the spider’s <c>parse()</c> method will be used.
        /// Note that if exceptions are raised during processing, <c>errback</c> is called instead.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        [JsonProperty("callback")]
        public string Callback { get; set; }

        /// <summary>
        /// The HTTP method used in the request.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [JsonProperty("method")]
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Contains arbitrary metadata for this request. This dict is empty for new Requests,
        /// and is usually populated by different Scrapy components (extensions, middleware, etc).
        /// So the data contained in this dictionary depends on the extensions you have enabled.
        /// You can also pass <c>ScrapyRequestMetaSpecialKey</c> constants for the keys.
        /// </summary>
        /// <seealso cref="http://doc.scrapy.org/en/latest/topics/request-response.html#request-meta-special-keys"/>
        /// <value>
        /// The meta.
        /// </value>
        [JsonProperty("meta")]
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// The request body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// The headers of this request. The dictionary values can be strings
        /// (for single valued headers) or lists (for multi-valued headers).
        /// If <c>None</c> is passed as value, the HTTP header will not be sent at all.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        [JsonProperty("headers")]
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        /// Gets or sets the cookies.
        /// You can use a string for a cookie value or pass a <c>TwistedCookieValue</c>
        /// to further customize the domain and path.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        [JsonProperty("cookies")]
        public Dictionary<string, object> Cookies { get; set; }

        /// <summary>
        /// The encoding of this request (defaults to <c>'utf-8'</c>).
        /// This encoding will be used to percent-encode the URL and to convert
        /// the body to <c>str</c> (if given as <c>unicode</c>).
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        /// <summary>
        /// The priority of this request (defaults to 0).
        /// The priority is used by the scheduler to define the order used to process requests.
        /// Requests with a higher priority value will execute earlier.
        /// Negative values are allowed in order to indicate relatively low-priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [JsonProperty("priority")]
        public long Priority { get; set; }

        /// <summary>
        /// Indicates that this request should not be filtered by the scheduler.
        /// This is used when you want to perform an identical request multiple times,
        /// to ignore the duplicates filter. Use it with care, or you will get into crawling loops. Default to False.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do not filter]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("dont_filter")]
        public bool? DoNotFilter { get; set; }

        /// <summary>
        /// A function that will be called if any exception was raised while processing the request.
        /// This includes pages that failed with 404 HTTP errors and such.
        /// It receives a Twisted Failure instance as first parameter. 
        /// </summary>
        /// <value>
        /// The error fallback function name.
        /// </value>
        [JsonProperty("errback")]
        public string ErrorFallbackFunction { get; set; }

        /// <summary>
        /// Flags sent to the request, can be used for logging or similar purposes.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        [JsonProperty("flags")]
        public List<string> Flags { get; set; }

        /// <summary>
        /// A dictionary with arbitrary data that will be passed as keyword arguments to the Request’s callback.
        /// </summary>
        /// <value>
        /// The callback arguments.
        /// </value>
        [JsonProperty("cb_kwargs")]
        public Dictionary<string, object> CallbackArguments { get; set; }
    }
}