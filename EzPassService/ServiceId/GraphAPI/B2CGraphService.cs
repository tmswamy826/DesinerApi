using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Configuration;
using Common;

namespace EzPassService.ServiceId.GraphAPI
{
    /// <summary>
    /// B2CGraphService used to grab the user attributes definied in the Azure B2C instance
    /// Id's and Secrets are defined in the app.config file
    /// </summary>
    public class B2CGraphService : IB2CGraphService
    {
        private const string ClientId = "5396b536-cb32-4efe-89fe-99157f12aae1";
        private const string ClientSecret = "0stTPe3xwdk/gw3h1myQC5s1gT3Qf4yyKrKtdBmjff8=";
        private const string Tenant = "ezpassdirectory.onmicrosoft.com";

        private const string AadInstance = "https://login.microsoftonline.com/";
        private const string AadGraphEndpoint = "https://graph.windows.net/";
        private const string AadGraphVersion = "api-version=1.6";

        private readonly IServiceClient _client;

        public B2CGraphService(IServiceClient serviceClient)
        {
            _client = serviceClient;
        }

        /// <summary>
        /// Get the User by the Object Id
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async Task<string> GetUserByObjectId(string objectId)
        {
            return await SendGraphGetRequest("/users/" + objectId, null);
        }

        /// <summary>
        /// Send the Graph Request to the Azure B2C instance
        /// </summary>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<string> SendGraphGetRequest(string api, string query)
        {
            // The AuthenticationContext is ADAL's primary class, in which you indicate the directory to use.
            var authContext = new AuthenticationContext(AadInstance + Tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are 
            // provided to Azure AD in order to receive an access_token using the app's identity.
            var credential = new ClientCredential(ClientId, ClientSecret);


            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            var result = authContext.AcquireTokenAsync(AadGraphEndpoint, credential).Result;

            // For B2C user management, be sure to use the 1.6 Graph API version.
            var url = AadGraphEndpoint + Tenant + api + "?" + AadGraphVersion;
            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            var error = await response.Content.ReadAsStringAsync();
            var formatted = JsonConvert.DeserializeObject(error);
            throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));

        }
    }
}
