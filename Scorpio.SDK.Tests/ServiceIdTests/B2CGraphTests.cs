using System.Net;
using System.Net.Http;
using System.Text;
using Common.SDK.Tests;
using EzPassService.ServiceId.GraphAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Scorpio.SDK.Tests.ServiceIdTests
{
    /// <summary>
    /// Tests the B2CGraphService Class
    /// </summary>
    public class B2CGraphTests
    {
        public static readonly JObject TestObj = new JObject()
        {
            new JProperty("Test","123")
        };

        private readonly FakeHttpMessageHandler _goodMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(TestObj), Encoding.UTF8, "application/json")
        });

        /// <summary>
        /// Tests the GetUserByObjectId path
        /// </summary>
        [Fact]
        public void GetUserByObjectIdTest()
        {
            var mockServiceClient = new MockServiceClient(_goodMessageHandler);
            var b2CGraphService = new B2CGraphService(mockServiceClient);

            var expected = JsonConvert.SerializeObject(TestObj);
            var response = b2CGraphService.GetUserByObjectId("").Result;

            Assert.Equal(expected,response);
        }
    }
}
