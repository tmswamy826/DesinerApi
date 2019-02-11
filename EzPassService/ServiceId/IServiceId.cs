using System.Threading.Tasks;

namespace EzPassService.ServiceId
{
    public interface IServiceId
    {
        Task<string> GetServiceId(string serviceUrl, string userToken);
    }
}