using System;
using System.Net;
using Common.SDK.Tests;
using EzPassService.ServiceId.GraphAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using EzPassService.ServiceId;
using Scorpio.SDK.Tests.ServiceIdTests;
using Xunit;

namespace Scorpio.SDK.Tests
{
    /// <summary>
    /// Tests the EzPassService Class
    /// </summary>
    public class EzPassServiceTest
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
        private static readonly MockServiceClient MockServiceClientServiceId = new MockServiceClient(MessageHandlerServiceId);
        private static readonly MockGraphService MockGraphService = new MockGraphService();
        private static readonly MockB2CGraphService MockB2CGraphService = new MockB2CGraphService();
        private readonly ServiceId _serviceId = new ServiceId(MockGraphService, MockB2CGraphService, MockServiceClientServiceId);

        /// <summary>
        /// Tests the Constructor
        /// </summary>
        [Fact]
        public void EzPassServiceConstructor()
        {
            var ezPassService = new EzPassService.EzPassService();
            Assert.NotNull(ezPassService);
        }

        /// <summary>
        /// Tests the successful call service function with Account Details and GetAccountDetails
        /// </summary>
        [Fact]
        public void CallServiceTest()
        {
            var testJObject = new JObject()
            {
                new JProperty("statusCode","0"),
                new JProperty("message","Success"),
            };

            var expectedOutput = testJObject.ToString();

            var input = new JObject()
            {
                new JProperty("token", "abc123"),
                new JProperty("serviceUrl", ServiceUrl.Url)
            };
            var outputReference = new JObject()
            {
                new JProperty("statusCode"),
                new JProperty("message"),
            };
            var messageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(testJObject), Encoding.UTF8, "application/json")
            });

            var mockServiceClient = new MockServiceClient(messageHandler);

            var ezPassService = new EzPassService.EzPassService(mockServiceClient,MockGraphService,MockB2CGraphService, _serviceId);

            ezPassService.CallService("AccountDetails", "GetAccountDetails", input, ref outputReference);

            Assert.Equal(expectedOutput, outputReference.ToString());
        }

        /// <summary>
        /// Tests the path where the Client provides a bad output form
        /// </summary>
        [Fact]
        public void CallServiceTestBadOutput()
        {
            var testJObject = new JObject()
            {
                new JProperty("statusCode","0"),
                new JProperty("message","Success"),
            };
            var input = new JObject()
            {
                new JProperty("token", "abc123"),
                new JProperty("serviceUrl", ServiceUrl.Url)
            };
            var outputReference = new JObject()
            {
            };
            var messageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(testJObject), Encoding.UTF8, "application/json")
            });

            var mockServiceClient = new MockServiceClient(messageHandler);

            var ezPassService = new EzPassService.EzPassService(mockServiceClient, MockGraphService, MockB2CGraphService, _serviceId);

            Assert.Throws<ArgumentException>(() =>
                ezPassService.CallService("AccountDetails", "GetAccountDetails", input, ref outputReference));
        }

        /// <summary>
        /// Tests the path where the client provides a bad input
        /// </summary>
        [Fact]
        public void CallServiceTestBadInput()
        {
            var testJObject = new JObject()
            {
                new JProperty("statusCode","0"),
                new JProperty("message","Success"),
            };
            var input = new JObject()
            {
            };
            var outputReference = new JObject()
            {
            };
            var messageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(testJObject), Encoding.UTF8, "application/json")
            });

            var mockServiceClient = new MockServiceClient(messageHandler);

            var ezPassService = new EzPassService.EzPassService(mockServiceClient, MockGraphService, MockB2CGraphService, _serviceId);

            Assert.Throws<ArgumentException>(() =>
                ezPassService.CallService("AccountDetails", "GetAccountDetails", input, ref outputReference));
        }

        /// <summary>
        /// Tests the path where the client provides a bad method name
        /// </summary>
        [Fact]
        public void CallServiceBadMethodName()
        {
            var messageHandler = new FakeHttpMessageHandler();

            var mockServiceClient = new MockServiceClient(messageHandler);

            var ezPassService = new EzPassService.EzPassService(mockServiceClient, MockGraphService, MockB2CGraphService, _serviceId);
            var input = new JObject();
            var outputReference = new JObject();
            
            Assert.Throws<ArgumentException>(() =>
                ezPassService.CallService("AccountDetails", "BadMethodName", input, ref outputReference));
        }

        /// <summary>
        /// Tests the path where the client provides a bad class name
        /// </summary>
        [Fact]
        public void CallServiceBadClassName()
        {
            var messageHandler = new FakeHttpMessageHandler();

            var mockServiceClient = new MockServiceClient(messageHandler);

            var ezPassService = new EzPassService.EzPassService(mockServiceClient, MockGraphService, MockB2CGraphService, _serviceId);
            var input = new JObject();
            var outputReference = new JObject();
            ;
            Assert.Throws<ArgumentException>(() =>
                ezPassService.CallService("BadClassName", "BadMethodName", input, ref outputReference));
        }
    }
}
