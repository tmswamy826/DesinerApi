using System.Threading.Tasks;

namespace EzPassService.ServiceId.GraphAPI
{
    public interface IGraphService
    {
        Task<string> GetUserId(string userToken);
    }
}