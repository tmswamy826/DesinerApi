using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using BigBazarService.ServiceId;
using BigBazarService.ServiceId.GraphAPI;
using Common;

namespace BigBazarService
{
    public class BigBazarService 
    {
        private readonly List<Object> _services;

        public BigBazarService()
        {
            IServiceClient serviceClient = new ServiceClient();
            IB2CGraphService b2CGraphService = new B2CGraphService(serviceClient);
            IGraphService graphService = new GraphService();
            
#if DEBUG
            IServiceId serviceId = new ServiceId.ServiceId(graphService, b2CGraphService, serviceClient);
          //  IServiceId serviceId = new ServiceId.MockServiceId(graphService, b2CGraphService, serviceClient);
#else
            IServiceId serviceId = new ServiceId.ServiceId(graphService, b2CGraphService, serviceClient);
#endif

            _services = new List<object>()
            {
               // new TollTag(serviceId,serviceClient),
               // new AccountDetails(serviceId, serviceClient),
               
            };
        }
    }
}
