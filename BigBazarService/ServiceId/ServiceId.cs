using Newtonsoft.Json;
using BigBazarService.ServiceId.AuthorizeUser;
using BigBazarService.ServiceId.LoginUser;
using BigBazarService.ServiceId.GraphAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace BigBazarService.ServiceId
{
    class ServiceId : IServiceId
    {
        private readonly IServiceClient _serviceClient;

        /// <summary>
        /// Instantiate the GraphAPIService
        /// </summary>
        private readonly IGraphService _graphService;

        /// <summary>
        /// Instantiate the B2CGraphService
        /// </summary>
        private readonly IB2CGraphService _b2CGraphService;

      

        public ServiceId(IGraphService graphService, IB2CGraphService b2CGraphService, IServiceClient serviceClient)
        {
            _graphService = graphService;
            _b2CGraphService = b2CGraphService;
            _serviceClient = serviceClient;
        }
        /// <summary>
        /// Main service entry point
        /// Makes calls to get user attributes and pass them to the EzPass authorization flow
        /// </summary>
        /// <param name="serviceUrl">Service Url of the EzPass Endpoint</param>
        /// <param name="userToken">User's token</param>
        /// <returns></returns>
        public async Task<string> GetServiceId(string serviceUrl, string userToken)
        {

            if (string.IsNullOrEmpty(userToken))
            {
                throw new ArgumentException(string.Format("Token is either empty or null: {0}", userToken));
            }
            var userAttributes = GetUserAttributes(userToken);

            return await GetServiceId(serviceUrl, userAttributes);
        }

        /// <summary>
        /// Helper function to authenticate the user using the token and return the user attributes
        /// </summary>
        private IEnumerable<KeyValuePair<string, object>> GetUserAttributes(string token)
        {
            // Initialize the GraphServiceClient.
            var user = _graphService.GetUserId(token).Result;

            // Initialize the B2CGraphClient
            var res = JsonConvert.DeserializeObject<Dictionary<string, object>>(_b2CGraphService.GetUserByObjectId(user).Result);

            //Get User Attributes
            var userAttributes = res.Where(item => item.Key.Contains("extension"));

            return userAttributes;
        }

        /// <summary>
        /// Function to get the serviceId based on the userAttributes
        /// Makes a call to EzPass LoginUser, then a call to EzPass AuthorizeUser using the ServiceClient from Common
        /// </summary>
        /// <param name="serviceUrl">Service Url endpoint</param>
        /// <param name="userAttributes">User Attributes</param>
        /// <returns></returns>
        protected async Task<string> GetServiceId(string serviceUrl, IEnumerable<KeyValuePair<string, object>> userAttributes)
        {
            var keyValuePairs = userAttributes.ToList();
            var loginUser = new LoginUserRequest()
            {
                username = keyValuePairs.FirstOrDefault(item => item.Key.Contains("UserName")).Value?.ToString(),
                password = keyValuePairs.FirstOrDefault(item => item.Key.Contains("Password")).Value?.ToString(),
                vendorId = keyValuePairs.FirstOrDefault(item => item.Key.Contains("VendorId")).Value?.ToString(),
                model = "TEST",
                systemVersion = "1.0",
                appVersion = "1.0",
                verificationToken = keyValuePairs.FirstOrDefault(item => item.Key.Contains("VerificationToken")).Value?.ToString(),
            };
            var loginUserResponse = await _serviceClient.PostAsync<LoginUserResponse>(serviceUrl, loginUser);

            var authorizeUser = new AuthorizeUserRequest()
            {
                vendorId = loginUser.vendorId,
                verificationToken = Sha256(loginUser.vendorId + "|" + loginUserResponse.LoginUser.AccessId + "|" + "XeroxMobileApp"),
                accessId = loginUserResponse.LoginUser.AccessId,
            };

            var authorizeUserResponse = await _serviceClient.PostAsync<AuthorizeUserResponse>(serviceUrl, authorizeUser);

            var serviceId = authorizeUserResponse.AuthorizeUser.ServiceId;
            return serviceId;
        }

        /// <summary>
        /// Sha 256 Hash generator for the Authorize User call
        /// </summary>
        /// <param name="randomString"></param>
        /// <returns></returns>
        private static string Sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (var theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
