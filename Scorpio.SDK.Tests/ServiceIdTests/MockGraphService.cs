using System.Threading.Tasks;
using EzPassService.ServiceId.GraphAPI;

namespace Scorpio.SDK.Tests.ServiceIdTests
{
    /// <summary>
    /// Mock Graph Service Class to return a fake User Id
    /// </summary>
    public class MockGraphService : IGraphService
    {
        public Task<string> GetUserId(string userToken)
        {
            return Task.FromResult("fakeUserId");
        }
    }
}
