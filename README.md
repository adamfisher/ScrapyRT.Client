
# ScrapyRT.Client [![](https://raw.githubusercontent.com/pixel-cookers/built-with-badges/master/nuget/nuget-long.png)](https://www.nuget.org/packages/ScarpyRT.Client)
A strongly-typed C# client to make calls to a scrapyrt (Scrapy real-time) HTTP endpoint.

Please see [scrapyrt documentation](https://scrapyrt.readthedocs.io/en/latest/index.html) for complete details on making requests.

## Getting Started

You can initialize a new scrapyrt client by passing the base address to the location where your server is running:

```csharp
var client = new ScrapyRTClient("http://localhost:9080");
```

... or by passing your own `HttpClient` if you want more control over outgoing requests:

```csharp
var client = new ScrapyRTClient(new HttpClient() {BaseAddress = new Uri("http://localhost:9080")});
```

Assume we have an item model that correlates to the structure of the items scraped by scrapy:

```csharp
public class CountryItem
{
	public string CountryName { get; set; }
}
```

### GET Requests

The simplest way to get items from the scrapyrt endpoint is using a `GET` request. The following examples show how we call **ExampleSpider** with the url to be scraped:

Get a single item:

```csharp
CountryItem response = await client.GetSpiderSingleItemAsync<CountryItem>("ExampleSpider", "http://example.webscraping.com");
```

Get a list of items:

```csharp
List<CountryItem> response = await client.GetSpiderItemsAsync<CountryItem>("ExampleSpider", "http://example.webscraping.com");
```

Get the complete crawl response including crawl stats:

```csharp
CrawlResponse<CountryItem> response = await client.GetSpiderCrawlAsync<CountryItem>("ExampleSpider", "http://example.webscraping.com");
```

### POST Requests

Making a `POST` request allows you to specify more advanced configuration for each call. The following examples show how we call **ExampleSpider** with the url to be scraped.

Get a single item:

```csharp
CountryItem response = await client.PostSpiderSingleItemAsync<CountryItem>(new CrawlRequest()
{
    SpiderName = "ExampleSpider",
    Request = new TwistedRequest()
    {
        Url = new Uri("http://example.webscraping.com")
    }
});
```

Get a list of items:

```csharp
List<CountryItem> response = await client.PostSpiderItemsAsync<CountryItem>(new CrawlRequest()
{
    SpiderName = "ExampleSpider",
    Request = new TwistedRequest()
    {
        Url = new Uri("http://example.webscraping.com")
    }
});
```

Get the complete crawl response including crawl stats:

```csharp
CrawlResponse<CountryItem> response = await client.PostSpiderCrawlAsync<CountryItem>(new CrawlRequest()
{
    SpiderName = "ExampleSpider",
    Request = new TwistedRequest()
    {
        Url = new Uri("http://example.webscraping.com")
    }
});
```

There are tons of other options available to customize how scrapy's Twisted networking library makes the request on your behalf. Here we specify an `X-Example-Header` that should be passed when scrapy downloads the web page and to return no more than 3 results in the response:

```csharp
List<CountryItem> response = await client.PostSpiderItemsAsync<CountryItem>(new CrawlRequest()
{
    SpiderName = "ExampleSpider",
    MaxRequests = 3,
    Request = new TwistedRequest()
    {
        Url = new Uri("http://example.webscraping.com"),
        Headers = new Dictionary<string, object>()
        {
            {"X-Example-Header", "Scrapy"}
        }
    }
});
```
