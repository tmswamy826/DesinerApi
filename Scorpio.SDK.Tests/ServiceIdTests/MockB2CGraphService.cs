using System.Collections.Generic;
using System.Threading.Tasks;
using EzPassService.ServiceId.GraphAPI;
using Newtonsoft.Json;

namespace Scorpio.SDK.Tests.ServiceIdTests
{
    /// <summary>
    /// Mock B2CGraphService to return a fake user
    /// </summary>
    public class MockB2CGraphService : IB2CGraphService
    {
        public Task<string> GetUserByObjectId(string objectId)
        {
            var fakeUserAttributes = new Dictionary<string, object>()
            {
                {"extension_Vendor", "TEST123"},
                {"extension_UserName", "32610095"},
                {"extension_Password", "Welcome1"},
                {"extension_verificationToken", "0e0cd73a0c5db436bcfd7f1a32276e882009119ddc62d2089565d65ae77498da"}
            };
            var result = JsonConvert.SerializeObject(fakeUserAttributes);
            return Task.FromResult<string>(result);
        }
    }
}
