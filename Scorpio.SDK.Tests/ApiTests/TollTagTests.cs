using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Common;
using Common.SDK.Tests;
using EzPassService.ServiceId;
using EzPassService.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scorpio.SDK.Tests.ServiceIdTests;
using Xunit;

namespace Scorpio.SDK.Tests.ApiTests
{
    /// <summary>
    /// Tests the code paths with the TollTag Class
    /// </summary>
    public class TollTagTests
    {
        private static readonly JObject TestJObjectServiceId = new JObject()
        {
            new JProperty("LoginUser",new JObject()
            {
                new JProperty("AccessId", "abc123")
            }),
            new JProperty("AuthorizeUser", new JObject()
            {
                new JProperty("ServiceId", "abc123")
            })
        };

        private static readonly FakeHttpMessageHandler MessageHandlerServiceId = new FakeHttpMessageHandler(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(TestJObjectServiceId), Encoding.UTF8, "application/json")
        });
        private readonly MockServiceClient _mockServiceClientServiceId = new MockServiceClient(MessageHandlerServiceId);

        /// <summary>
        /// Tests the successful order toll tag path
        /// </summary>
        [Fact]
        public void OrderTollTagTest()
        {
            var testJObject = new JObject()
            {
                new JProperty("statusCode","0"),
                new JProperty("message","Success"),
            };

            var messageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(testJObject), Encoding.UTF8, "application/json")
            });

            var mockServiceClient = new MockServiceClient(messageHandler);
            var mockGraphService = new MockGraphService();
            var mockB2CGraphService = new MockB2CGraphService();
            var serviceId = new ServiceId(mockGraphService, mockB2CGraphService, _mockServiceClientServiceId);

            var tollTag = new TollTag(serviceId, mockServiceClient);
            var input = new JObject()
            {
                new JProperty("token", "123"),
                new JProperty("serviceUrl", ServiceUrl.Url),
                new JProperty("quantity", "1"),
                new JProperty("transponderType", "interior")
            };
            var expectedOutput = new JObject()
            {
                new JProperty("statusCode", "0"),
                new JProperty("message", "Success")
            };

            var response = tollTag.OrderTollTag(input);
            var responseObj = JObject.Parse(response);
            foreach (var item in responseObj)
            {
                Assert.True(expectedOutput.ContainsKey(item.Key));
                var value = expectedOutput.GetValue(item.Key);
                Assert.Equal(value, item.Value);
            }
        }
    }
}
