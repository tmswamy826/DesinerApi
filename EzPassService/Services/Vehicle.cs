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
    /// Vehicle Service to describe any calls associated with Vehicles
    /// Extends the ServiceClient to fulfill Http requests
    /// Implements IService
    /// </summary>
    public class Vehicle
    {
        private readonly IServiceId _serviceId;
        private readonly IServiceClient _serviceClient;
        public Vehicle(IServiceId serviceId, IServiceClient serviceClient)
        {
            _serviceId = serviceId;
            _serviceClient = serviceClient;
        }

        /// <summary>
        /// Method to invoke to add a vehicle
        /// </summary>
        /// <param name="input">Input parameters for the service call</param>
        /// <returns></returns>
        public string AddVehicle(JObject input)
        {
            var inputFormat = new JObject()
            {
                new JProperty("token"),
                new JProperty("serviceUrl"),
                new JProperty("plateNumber"),
                new JProperty("plateState"),
            };

            Validation.ValidateInput(input, inputFormat);

            var token = input.GetValue("token").ToString();
            var serviceUrl = input.GetValue("serviceUrl").ToString();
            var serviceId = _serviceId.GetServiceId(serviceUrl, token).Result;

            var request = new JObject()
            {
                new JProperty("action","addVehicleList"),
                new JProperty("serviceId", serviceId),
                new JProperty("plateNumber",input.GetValue("plateNumber")),
                new JProperty("plateSate",input.GetValue("plateState")),
                new JProperty("vehicleYear","2014"),
                new JProperty("vehicleMake","ACURA"),
                new JProperty("vehicleModel","SAFAF"),
                new JProperty("vehicleCountry","USA"),
                new JProperty("vehicleStartdate","4/10/2014 04:05:00"),
                new JProperty("vehicleEnddate","4/17/2014 04:05:00"),
                new JProperty("plateTypedesc","STANDARD"),
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
