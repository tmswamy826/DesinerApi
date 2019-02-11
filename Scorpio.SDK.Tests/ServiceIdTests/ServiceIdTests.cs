using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.SDK.Tests;
using EzPassService.ServiceId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Scorpio.SDK.Tests.ServiceIdTests
{
    /// <summary>
    /// Tests the ServiceId Class
    /// </summary>
    public class ServiceIdTests
    {
        /// <summary>
        /// Tests the Get ServiceId function
        /// </summary>
        [Fact]
        public void GetServiceIdTest()
        {
            var testJObject = new JObject()
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

            var messageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(testJObject), Encoding.UTF8, "application/json")
            });

            var mockServiceClient = new MockServiceClient(messageHandler);
            var mockGraphService = new MockGraphService();
            var mockB2CGraphService = new MockB2CGraphService();

            var serviceId = new ServiceId(mockGraphService, mockB2CGraphService, mockServiceClient);

            const string expectedOutput = "abc123";
            var result = serviceId.GetServiceId(ServiceUrl.Url, "fakeToken").Result;
            Assert.Equal(expectedOutput, result);
        }

        /// <summary>
        /// Tests the get service Id path with a bad token
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetServiceIdTestBadTokenAsync()
        {

            var messageHandler = new FakeHttpMessageHandler();

            var mockServiceClient = new MockServiceClient(messageHandler);
            var mockGraphService = new MockGraphService();
            var mockB2CGraphService = new MockB2CGraphService();

            var serviceId = new ServiceId(mockGraphService, mockB2CGraphService, mockServiceClient);
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceId.GetServiceId(ServiceUrl.Url, ""));
        }
    }
}
