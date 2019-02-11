using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.SDK.Tests
{
    /// <summary>
    /// Mock ServiceClient that creates a fake message handler that will process requests
    /// </summary>
    public class MockServiceClient : ServiceClient
    {
        public MockServiceClient(FakeHttpMessageHandler fakeHttpMessageHandler) : base(new HttpClient(fakeHttpMessageHandler))
        {
        }
    }

    /// <summary>
    /// Fake Message Handler to be passed into an Http Client
    /// </summary>
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage _fakeResponse;
        private readonly bool _timeout = false;
        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        public FakeHttpMessageHandler(bool timeout)
        {
            _timeout = timeout;
        }

        public FakeHttpMessageHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_timeout)
                throw new TaskCanceledException();
            return await Task.FromResult(_fakeResponse);
        }
    }
}
