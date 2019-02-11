using System.Threading.Tasks;

namespace BigBazarService.ServiceId.GraphAPI
{
    public interface IB2CGraphService
    {
        Task<string> GetUserByObjectId(string objectId);
    }
}