using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    public class MongodbService
    {
        public HttpClient Client { get; }

        public MongodbService(HttpClient client)
        {
            //client.BaseAddress = new Uri("http://localhost:6004/api/token/");
            //添加ocelot后统一网关
            client.BaseAddress = new Uri("http://localhost:5000/mongodbservice/");
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
    }
}
