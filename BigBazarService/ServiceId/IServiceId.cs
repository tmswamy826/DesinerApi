using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigBazarService.ServiceId
{
    public interface IServiceId
    {
        Task<string> GetServiceId(string serviceUrl, string userToken);
    }
}
