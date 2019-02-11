using System.Net.Http;
using System.Threading.Tasks;

namespace Common
{
    public interface IServiceClient
    {
        Task<T> DeleteAsync<T>(string serviceUrl);
        Task<T> GetAsync<T>(string serviceUrl);
        Task<T> PostAsync<T>(string serviceUrl, object data);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}