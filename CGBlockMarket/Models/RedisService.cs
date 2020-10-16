using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    public class RedisService
    {
        public HttpClient Client { get; }

        public RedisService(HttpClient client)
        {
            //client.BaseAddress = new Uri("http://localhost:6001/api/redis/");

            ////添加ocelot后统一网关
            client.BaseAddress = new Uri("http://localhost:5000/redisservice/");

            Client = client;
        }

        public async Task<string> ServiceHttpGet(string url)
        {
            var response = await Client.GetAsync(
                url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            else
            {
                return "error请求失败";
            }
        }

        
        public async Task<string> ServiceHttpPost(string url,string content)
        {

            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Client.PostAsync(
                url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            else
            {
                return "error请求失败";
            }
        }
    }
}
