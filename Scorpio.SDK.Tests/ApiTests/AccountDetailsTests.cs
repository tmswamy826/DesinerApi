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
    /// Tests the code paths with the Account Details Class
    /// </summary>
    public class AccountDetailsTests
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
        /// Get Account Details Successful Path
        /// </summary>
        [Fact]
        public void GetAccountDetailsTest()
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

            var accountDetails = new AccountDetails(serviceId, mockServiceClient);
            var input = new JObject()
            {
                new JProperty("token", "abc123"),
                new JProperty("serviceUrl", ServiceUrl.Url)
            };
            var expectedOutput = new JObject()
            {
                new JProperty("statusCode", "0"),
                new JProperty("message", "Success")
            };

            var response = accountDetails.GetAccountDetails(input);
            var responseObj = JObject.Parse(response);
            foreach (var item in responseObj)
            {
                Assert.True(expectedOutput.ContainsKey(item.Key));
                var value = expectedOutput.GetValue(item.Key);
                Assert.Equal(value, item.Value);
            }
        }

        /// <summary>
        /// Get Current Balance Successful Path
        /// </summary>
        [Fact]
        public void GetCurrentBalanceTest()
        {
            var testJObject = new JObject()
            {
                new JProperty("statusCode","0"),
                new JProperty("message","Success"),
                new JProperty("accountDetail", new JObject()
                {
                    new JProperty("currentBalance","123")
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
            var serviceId = new ServiceId(mockGraphService, mockB2CGraphService, _mockServiceClientServiceId);

            var accountDetails = new AccountDetails(serviceId, mockServiceClient);
            var input = new JObject()
            {
                new JProperty("token", "123"),
                new JProperty("serviceUrl", ServiceUrl.Url)
            };
            var expectedOutput = new JObject()
            {
                new JProperty("statusCode", "0"),
                new JProperty("message", "Success"),
                new JProperty("currentBalance", "123")
            };

            var response = accountDetails.GetCurrentBalance(input);
            var responseObj = JObject.Parse(response);
            foreach(var item in responseObj)
            {
                Assert.True(expectedOutput.ContainsKey(item.Key));
                var value = expectedOutput.GetValue(item.Key);
                Assert.Equal(value, item.Value);
            }
        }
    }
}
