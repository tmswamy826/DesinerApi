using EzPassService.ServiceId;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using EzPassService.ServiceId.GraphAPI;

namespace EzPassService.ServiceId
{
    /// <summary>
    /// FOR DEVELOPMENT ONLY:
    /// Class used to grab a service Id using a default test user
    /// This should only be called from a debug build and using the test console
    /// </summary>
    public class MockServiceId : ServiceId, IServiceId
    {
        public MockServiceId(IGraphService graphService, IB2CGraphService b2CGraphService, IServiceClient serviceClient) : base(graphService, b2CGraphService, serviceClient)
        {
        }

        /// <summary>
        /// Service Id Wrapper
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public new Task<string> GetServiceId(string serviceUrl, string userToken)
        {
            return GetDefaultServiceId(serviceUrl);
        }

        /// <summary>
        /// FOR DEVELOPMENT ONLY: Test function to get default user-attributes. 
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        private Task<string> GetDefaultServiceId(string serviceUrl)
        {
            var userAttributes = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("UserName", "32610095"),
                new KeyValuePair<string, object>("Password", "Welcome1"),
                new KeyValuePair<string, object>("VendorId", "TEST123"),
                new KeyValuePair<string, object>("VerificationToken", "0e0cd73a0c5db436bcfd7f1a32276e882009119ddc62d2089565d65ae77498da")
            };

            return this.GetServiceId(serviceUrl, userAttributes);
        }
    }
}
