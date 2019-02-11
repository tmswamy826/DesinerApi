using Microsoft.Graph;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace EzPassService.ServiceId.GraphAPI
{
    /// <summary>
    /// Microsoft Graph API Service to retrieve the User's Id
    /// </summary>
    public class GraphService : IGraphService
    {
        /// <summary>
        /// Gets an authenticated Graph Service Client
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private static GraphServiceClient GetAuthenticatedClient(string userToken)
        {
             return new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage => await Task.Run(() =>
                {
                    // Passing tenant ID to the sample auth provider to use as a cache key
                    var accessToken = userToken;

                    // Append the access token to the request
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                })));
        }

        /// <summary>
        /// Grabs User's profile and returns the ID
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public async Task<string> GetUserId(string userToken)
        {
            var graphClient = GetAuthenticatedClient(userToken);
            var user = await graphClient.Me.Request().GetAsync();  
            return user.Id;
        }
    }
}
