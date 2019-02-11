using System.Threading.Tasks;

namespace EzPassService.ServiceId.GraphAPI
{
    public interface IB2CGraphService
    {
        Task<string> GetUserByObjectId(string objectId);
    }
}