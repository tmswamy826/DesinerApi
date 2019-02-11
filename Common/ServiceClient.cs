using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Describes the service client
    /// Responsible for fulfilling all Http requests
    /// </summary>
    public class ServiceClient : IServiceClient
    {
        private readonly HttpClient _client;

        public ServiceClient()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
        }

        /// <summary>
        /// Constructor used to initialize the ServiceClient
        /// </summary>
        /// <param name="client"></param>
        public ServiceClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Post Async method to post data to a specific endpoint
        /// </summary>
        /// <typeparam name="T">Generic return type</typeparam>
        /// <param name="serviceUrl">Service URL endpoint</param>
        /// <param name="data">Data for Post</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string serviceUrl, object data)
        {
            T result;

            try
            {
                var uri = new Uri(serviceUrl);

                var jsonString = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/json");

                var res = await _client.PostAsync(uri, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync(); 
                    result = (typeof(T)==typeof(string)) ? (T)(object)content : JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new HttpRequestException(string.Format("Client Request was unsuccessful: {0}", res.StatusCode));
                }
            }
            catch (TaskCanceledException)
            {
                //Some Debug Exception message
                throw new TimeoutException("Server Request has timed out");
            }
            return result;
        }
        /// <summary>
        /// Get Async method to get data at a specific endpoint
        /// </summary>
        /// <typeparam name="T">Generic Return Type</typeparam>
        /// <param name="serviceUrl">Service URL endpoint</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string serviceUrl)
        {
            T result;

            try
            {
                var uri = new Uri(serviceUrl);

                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    result = (typeof(T) == typeof(string)) ? (T)(object)content : JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new HttpRequestException(string.Format("Client Request was unsuccessful: {0}", res.StatusCode));
                }
            }
            catch (TaskCanceledException)
            {
                //Some Debug Exception message
                throw new TimeoutException("Server Request has timed out");
            }
            return result;
        }
        /// <summary>
        /// Delete Async Method to delete data at a specified endpoint
        /// </summary>
        /// <typeparam name="T">Generic Return Type</typeparam>
        /// <param name="serviceUrl">Service URL endpoint</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string serviceUrl)
        {
            T result;

            try
            {
                var uri = new Uri(serviceUrl);
                var res = await _client.DeleteAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    result = (typeof(T) == typeof(string)) ? (T)(object)content : JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new HttpRequestException(string.Format("Client Request was unsuccessful: {0}", res.StatusCode));
                }
            }
            catch (TaskCanceledException)
            {
                //Some Debug Exception message
                throw new TimeoutException("Server Request has timed out");
            }
            return result;
        }

        /// <summary>
        /// Send Async Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                return await _client.SendAsync(request);
            }
            catch (TaskCanceledException)
            {
                //Some Debug Exception message
                throw new TimeoutException("Server Request has timed out");
            }
        }
    }
}
