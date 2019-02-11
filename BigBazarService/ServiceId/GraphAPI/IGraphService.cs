using System.Threading.Tasks;

namespace BigBazarService.ServiceId.GraphAPI
{
    public interface IGraphService
    {
        Task<string> GetUserId(string userToken);
    }
}