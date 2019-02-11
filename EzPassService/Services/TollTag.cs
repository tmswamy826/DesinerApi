using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using EzPassService.ServiceId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EzPassService.Services
{
    /// <summary>
    /// TollTag Service that describes any calls associated with TollTags
    /// Extends the ServiceClient to fulfill Http requests
    /// Implements IService
    /// </summary>
    public class TollTag 
    {
        private readonly IServiceId _serviceId;
        private readonly IServiceClient _serviceClient;
        public TollTag(IServiceId serviceId, IServiceClient serviceClient)
        {
            _serviceId = serviceId;
            _serviceClient = serviceClient;
        }

        /// <summary>
        /// Method to invoke to order a toll tag
        /// </summary>
        /// <param name="input">Input parameters for the service call</param>
        /// <returns></returns>
        public string OrderTollTag(JObject input)
        {
            var inputFormat = new JObject()
            {
                new JProperty("token"),
                new JProperty("serviceUrl"),
                new JProperty("quantity"),
                new JProperty("transponderType")
            };

            Validation.ValidateInput(input, inputFormat);

            var token = input.GetValue("token").ToString();
            var serviceUrl = input.GetValue("serviceUrl").ToString();
            var serviceId = _serviceId.GetServiceId(serviceUrl, token).Result;


            var request = new JObject()
            {
                new JProperty("action","orderTollTag"),
                new JProperty("serviceId", serviceId),
                new JProperty("quantity",input.GetValue("quantity")),
                new JProperty("transponderType",input.GetValue("transponderType")),
            };

            var response = JObject.Parse(_serviceClient.PostAsync<string>(serviceUrl, request).Result);
            var ret = new JObject()
            {
                new JProperty("statusCode", response.SelectToken("statusCode")),
                new JProperty("message", response.SelectToken("message")),
            };
            return JsonConvert.SerializeObject(ret);
        }
    }
}
