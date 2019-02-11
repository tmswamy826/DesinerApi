using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using System.Collections.Generic;
using System.Reflection;
using EzPassService.Services;
using EzPassService.ServiceId;
using EzPassService.ServiceId.GraphAPI;

namespace EzPassService
{
    /// <summary>
    /// Describes the overarching EzPass Service API entry-point
    /// Implements IServiceFactory
    /// </summary>
    public class EzPassService : IServiceFactory
    {
        /// <summary>
        /// Instantiates the list of service available to the client
        /// </summary>
        private readonly List<object> _services;

        /// <summary>
        /// Constructor used to initialize services to be used
        /// </summary>
        public EzPassService()
        {
            IServiceClient serviceClient = new ServiceClient();
            IB2CGraphService b2CGraphService = new B2CGraphService(serviceClient);
            IGraphService graphService = new GraphService();

#if DEBUG
            IServiceId serviceId = new ServiceId.ServiceId(graphService, b2CGraphService, serviceClient);
           // IServiceId serviceId = new ServiceId.MockServiceId(graphService, b2CGraphService, serviceClient);
#else
            IServiceId serviceId = new ServiceId.ServiceId(graphService, b2CGraphService, serviceClient);
#endif

            _services = new List<object>()
            {
                new TollTag(serviceId,serviceClient),
                new AccountDetails(serviceId, serviceClient),
                new Payment(serviceId, serviceClient),
                new Vehicle(serviceId, serviceClient)
            };
        }

        /// <summary>
        /// Constructor used in Unit testing to mock services
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="graphService"></param>
        /// <param name="b2CGraphService"></param>
        /// <param name="serviceId"></param>
        public EzPassService(IServiceClient serviceClient, IGraphService graphService, IB2CGraphService b2CGraphService, IServiceId serviceId)
        {
            var serviceClient1 = serviceClient;
            var b2CGraphService1 = b2CGraphService;
            var graphService1 = graphService;
            var serviceId1 = serviceId;

            _services = new List<object>()
            {
                new TollTag(serviceId1,serviceClient1),
                new AccountDetails(serviceId1, serviceClient1),
                new Payment(serviceId1, serviceClient1),
                new Vehicle(serviceId1, serviceClient1)
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// Require CallService function that invokes the intended serviceDefinition
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="input">Input Parameters</param>
        /// <param name="className"></param>
        /// <param name="output"></param>
        public void CallService(string className, string methodName, JObject input, ref JObject output)
        {
            try
            {
                foreach (var service in _services)
                {
                    if (service.GetType().Name != className) continue;
                    var serviceParams = new object[] {input};
                    var response = service.GetType().GetMethod(methodName).Invoke(service, serviceParams);
                    var responseObj = JObject.Parse(response.ToString());
                    Validation.ValidateOutput(output, responseObj);
                    output = responseObj;
                    return;
                }

                throw new ArgumentException(string.Format("Could not find class with name '{0}'", className));
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(string.Format("Could not find invocation with name '{0}'", methodName),
                    methodName);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
