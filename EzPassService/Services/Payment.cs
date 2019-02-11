using Common;
using EzPassService.ServiceId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzPassService.Services
{
    /// <summary>
    /// Payment Service to describe any calls associated with payments
    /// Extends the ServiceClient to fulfill Http requests
    /// Implements IService
    /// </summary>
    public class Payment
    {
        private readonly IServiceId _serviceId;
        private readonly IServiceClient _serviceClient;
        public Payment(IServiceId serviceId, IServiceClient serviceClient)
        {
            _serviceId = serviceId;
            _serviceClient = serviceClient;
        }

        /// <summary>
        /// Method to invoke to get the card on file
        /// </summary>
        /// <param name="input">Input parameters for the service call</param>
        /// <returns></returns>
        public string GetCardOnFile(JObject input)
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
                new JProperty("action","loadSavedCreditCard"),
                new JProperty("serviceId", serviceId)
            };
            var response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);
            var ret =  new JObject()
            {
                new JProperty("statusCode", response.SelectToken("statusCode")),
                new JProperty("message", response.SelectToken("message")),
                new JProperty("cardNumber", response.SelectToken("creditCardListType.cardsList[0].cardNumber"))
            };
            return JsonConvert.SerializeObject(ret);
        }

        public string PayWithCardOnFile(JObject input)
        {
            var inputFormat = new JObject()
            {
                new JProperty("token"),
                new JProperty("serviceUrl"),
                new JProperty("transactionAmount")
            };

            Validation.ValidateInput(input, inputFormat);

            var token = input.GetValue("token").ToString();
            var serviceUrl = input.GetValue("serviceUrl").ToString();
            var serviceId = _serviceId.GetServiceId(serviceUrl, token).Result;

            var request = new JObject()
            {
                new JProperty("action","loadSavedCreditCard"),
                new JProperty("serviceId", serviceId)
            };
            var response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);

            request.Remove("action");
            request.Add("action", "paymentVerificationCreditCardNew");
            request.Add("transactionAmount", input.GetValue("transactionAmount"));
            request.Add("rowId", response.SelectToken("creditCardListType.cardsList[0].rowId"));

            response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);
            var ret = new JObject()
            {
                new JProperty("statusCode", response.SelectToken("statusCode")),
                new JProperty("message", response.SelectToken("message")),
                new JProperty("transactionId", response.SelectToken("transactionId"))
            };
            return JsonConvert.SerializeObject(ret);
        }
    }
}
