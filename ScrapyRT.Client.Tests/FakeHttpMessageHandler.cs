using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapyRT.Client.Tests
{
    public class FakeHttpMessageHandler<TRepsonse> : FakeHttpMessageHandler.FakeHttpMessageHandler<TRepsonse> where TRepsonse : class
    {
        #region Properties

        public Uri ResponseUri { get; private set; }

        #endregion

        #region Constructors

        public FakeHttpMessageHandler(TRepsonse overrideResponseContent = null, Func<TRepsonse, string> serializerFunction = null) : base(overrideResponseContent, serializerFunction)
        {
        }

        public FakeHttpMessageHandler(Func<TRepsonse> createResponseObjectFunction, Func<TRepsonse, string> serializerFunction = null) : base(createResponseObjectFunction, serializerFunction)
        {
        }

        public FakeHttpMessageHandler(Func<Task<TRepsonse>> createResponseObjectAsyncFunction, Func<TRepsonse, string> serializerFunction = null) : base(createResponseObjectAsyncFunction, serializerFunction)
        {
        }

        #endregion

        #region Methods

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ResponseUri = request.RequestUri;
            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}
