using System;
using Xunit;
using Common;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Common.SDK.Tests
{
    /// <summary>
    /// Test the Code Paths within the ServiceClient Class
    /// </summary>
    public class ServiceClientTest
    {
        private static readonly JObject TestObj = new JObject()
        {
            new JProperty("Test","123")
        };

        private readonly FakeHttpMessageHandler _goodMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(TestObj), Encoding.UTF8, "application/json")
        });

        private readonly FakeHttpMessageHandler _badMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(JsonConvert.SerializeObject(TestObj), Encoding.UTF8, "application/json")
        });

        private readonly FakeHttpMessageHandler _timeoutMessageHandler = new FakeHttpMessageHandler(true);

        /// <summary>
        /// Test the Constructor
        /// </summary>
        [Fact]
        public void ServiceClientConstructor()
        {
            var serviceClient = new ServiceClient();
            Assert.NotNull(serviceClient);
        }

        /// <summary>
        /// Tests a Successful response path
        /// </summary>
        [Fact]
        public void ServiceClientPostAsyncTest()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = mockServiceClient.PostAsync<string>(url, TestObj.ToString()).Result;
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests a Successful response path where you return an object
        /// </summary>
        [Fact]
        public void ServiceClientPostAsyncTestObject()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = JsonConvert.SerializeObject(mockServiceClient.PostAsync<JObject>(url, TestObj.ToString()).Result);
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests an unsuccessful response path
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientPostAsyncTestBad()
        {
            const string url = "http://bad.uri";

            var mockServiceClient = new MockServiceClient(_badMessageHandler);
            await Assert.ThrowsAsync<HttpRequestException>(() => mockServiceClient.PostAsync<string>(url, TestObj.ToString()));
        }

        /// <summary>
        /// Tests the http-client timeout
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientPostAsyncTestTimeout()
        {
            const string url = "http://bad.uri";
            var mockServiceClient = new MockServiceClient(_timeoutMessageHandler);
            await Assert.ThrowsAsync<TimeoutException>(() => mockServiceClient.PostAsync<string>(url, TestObj.ToString()));
        }

        /// <summary>
        /// Tests a Successful response path
        /// </summary>
        [Fact]
        public void ServiceClientGetAsyncTest()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = mockServiceClient.GetAsync<string>(url).Result;
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests a Successful response path where you return an object
        /// </summary>
        [Fact]
        public void ServiceClientGetAsyncTestObject()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = JsonConvert.SerializeObject(mockServiceClient.GetAsync<JObject>(url).Result);
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests an unsuccessful response path
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientGetAsyncTestBad()
        {
            const string url = "http://bad.uri";

            var mockServiceClient = new MockServiceClient(_badMessageHandler);
            await Assert.ThrowsAsync<HttpRequestException>(() => mockServiceClient.GetAsync<string>(url));
        }

        /// <summary>
        /// Tests the http-client timeout
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientGetAsyncTestTimeout()
        {
            const string url = "http://bad.uri";
            var mockServiceClient = new MockServiceClient(_timeoutMessageHandler);
            await Assert.ThrowsAsync<TimeoutException>(() => mockServiceClient.GetAsync<string>(url));
        }

        /// <summary>
        /// Tests a Successful response path
        /// </summary>
        [Fact]
        public void ServiceClientDeleteAsyncTest()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = mockServiceClient.DeleteAsync<string>(url).Result;
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests a Successful response path where you return an object
        /// </summary>
        [Fact]
        public void ServiceClientDeleteAsyncTestObject()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var res = JsonConvert.SerializeObject(mockServiceClient.DeleteAsync<JObject>(url).Result);
            var result = JsonConvert.SerializeObject(TestObj);
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests an unsuccessful response path
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientDeleteAsyncTestBad()
        {
            const string url = "http://bad.uri";

            var mockServiceClient = new MockServiceClient(_badMessageHandler);
            await Assert.ThrowsAsync<HttpRequestException>(() => mockServiceClient.DeleteAsync<string>(url));
        }

        /// <summary>
        /// Tests the http-client timeout
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientDeleteAsyncTestTimeout()
        {
            const string url = "http://bad.uri";
            var mockServiceClient = new MockServiceClient(_timeoutMessageHandler);
            await Assert.ThrowsAsync<TimeoutException>(() => mockServiceClient.DeleteAsync<string>(url));
        }

        /// <summary>
        /// Tests a Successful response path
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientSendAsync()
        {
            const string url = "http://good.uri";

            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = JsonConvert.SerializeObject(TestObj);


            var response = await mockServiceClient.SendAsync(request);
            var res = await response.Content.ReadAsStringAsync();
            Assert.Equal(result, res);
        }

        /// <summary>
        /// Tests the http-client timeout
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task ServiceClientSendAsyncTimeout()
        {
            const string url = "http://bad.uri";
            var mockServiceClient = new MockServiceClient(_timeoutMessageHandler);
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            await Assert.ThrowsAsync<TimeoutException>(() => mockServiceClient.SendAsync(request));
        }

    }
}
