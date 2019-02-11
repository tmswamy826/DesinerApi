using System;
using System.Collections.Generic;
using System.Text;
using Common;
using EzPassService.ServiceId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EzPassService.Services
{
    /// <summary>
    /// Account Details Service that describes any calls associated with a user's account
    /// Extends the ServiceClient to fulfill Http requests
    /// Implements IService
    /// </summary>
    public class AccountDetails
    {
        private readonly IServiceId _serviceId;
        private readonly IServiceClient _serviceClient;
        public AccountDetails(IServiceId serviceId, IServiceClient serviceClient)
        {
            _serviceId = serviceId;
            _serviceClient = serviceClient;
        }

        /// <summary>
        /// Method to Invoke for Getting Account details
        /// </summary>
        /// <param name="input">Input parameters for the service call</param>
        /// <returns></returns>
        public string GetAccountDetails(JObject input)
        {

            var inputFormat = new JObject()
            {
                new JProperty("token"),
                new JProperty("serviceUrl"),
            };

            Validation.ValidateInput(input, inputFormat);

            var token = input.GetValue("token").ToString();
            var serviceUrl = input.GetValue("serviceUrl").ToString();
            var serviceId = _serviceId.GetServiceId(serviceUrl, token).Result;

            var request = new JObject()
            {
                new JProperty("action","getAccountDetails"),
                new JProperty("serviceId", serviceId),
            };
            var response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);
            var ret = new JObject()
            {
                new JProperty("statusCode", response.SelectToken("statusCode")),
                new JProperty("message", response.SelectToken("message")),
            };
            return JsonConvert.SerializeObject(ret);
        }

        /// <summary>
        /// Method to Invoke for getting the current balance
        /// </summary>
        /// <param name="input">Input parameters for the service call</param>
        /// <returns></returns>
        public string GetCurrentBalance(JObject input)
        {

            var inputFormat = new JObject()
            {
                new JProperty("token"),
                new JProperty("serviceUrl"),
            };

            Validation.ValidateInput(input, inputFormat);

            var token = input.GetValue("token").ToString();
            var serviceUrl = input.GetValue("serviceUrl").ToString();
            var serviceId = _serviceId.GetServiceId(serviceUrl, token).Result;

            var request = new JObject()
            {
                new JProperty("action","getAccountDetails"),
                new JProperty("serviceId", serviceId),
            };
            var response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);
            var ret = new JObject()
            {
                new JProperty("statusCode", response.SelectToken("statusCode")),
                new JProperty("message", response.SelectToken("message")),
                new JProperty("currentBalance", response.SelectToken("accountDetail.currentBalance"))
            };
            return JsonConvert.SerializeObject(ret);
        }
    }
}
