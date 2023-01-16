using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace CryptoInfo.Models
{
    class Asset
    {
        public string asset_id;
        public string name;
        public decimal price;
        public decimal volume_24h;
        public decimal change_1h;
        public decimal change_24h;
        public decimal change_7d;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;

        public Asset() { }
        public Asset(JToken token)
        {
            Console.Error.WriteLine(token);

            this.asset_id = token["asset_id"].ToString();
            this.name = token["name"].ToString();
            this.price = Convert.ToDecimal(token["price"]);
            this.volume_24h = Convert.ToDecimal(token["price"]);
            this.change_1h = Convert.ToDecimal(token["change_1h"]);
            this.change_24h = Convert.ToDecimal(token["change_24h"]);
            this.change_7d = Convert.ToDecimal(token["change_7d"]);
            
            this.status = token["status"].ToString();
            this.created_at = DateTime.Parse(token["created_at"].ToString());
            this.updated_at = DateTime.Parse(token["updated_at"].ToString());

        }
        public static async Task<Asset> FromIdAsync(string id)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://cryptingup.com/api/assets/{id}");
            Task<HttpResponseMessage> task = Task.Run(async () => await httpClient.SendAsync(request));
            HttpResponseMessage response = task.Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JToken token = JObject.Parse(await response.Content.ReadAsStringAsync());

                return new Asset(token["asset"]);
            }
            return new Asset();
        }
        public static Asset FromId(string id)
        {
            Task<Asset> task = Task.Run<Asset>(async () => await FromIdAsync(id));
            return task.Result;
        }
    }
    internal class Currency
    {
        public string name;
        public decimal price;
        public decimal volume_24h;
        public decimal market_cap;
        public decimal fully_diluted_market_cap;
    }
}
