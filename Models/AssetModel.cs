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
        public Currency[] quote;
        public string asset_id;
        public string name;
        public string description;
        public string website;
        public string ethereum_contract_address;
        public string pegged;
        public decimal price;
        public decimal volume_24h;
        public decimal change_1h;
        public decimal change_24h;
        public decimal change_7d;
        public decimal total_supply;
        public decimal circulating_supply;
        public decimal max_supply;
        public decimal market_cap;
        public decimal fully_diluted_market_cap;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;

        public Asset() { }
        public Asset(JToken token)
        {
            this.asset_id = token["asset_id"].ToString();
            this.name = token["name"].ToString();
            this.description = token["description"].ToString();
            this.website = token["website"].ToString();
            this.ethereum_contract_address = token["ethereum_contract_address"].ToString();
            this.pegged = token["pegged"].ToString();
            this.price = Convert.ToDecimal(token["price"]);
            this.volume_24h = Convert.ToDecimal(token["price"]);
            this.change_1h = Convert.ToDecimal(token["change_1h"]);
            this.change_24h = Convert.ToDecimal(token["change_24h"]);
            this.change_7d = Convert.ToDecimal(token["change_7d"]);
            this.total_supply = Convert.ToDecimal(token["total_supply"]);
            this.circulating_supply = Convert.ToDecimal(token["circulating_supply"]);
            this.max_supply = Convert.ToDecimal(token["max_supply"]);
            this.market_cap = Convert.ToDecimal(token["market_cap "]);
            this.fully_diluted_market_cap = Convert.ToDecimal(token["fully_diluted_market_cap"]);
            this.status = token["status"].ToString();
            this.created_at = DateTime.Parse(token["created_at"].ToString());
            this.updated_at = DateTime.Parse(token["updated_at"].ToString());

            List<Currency> currencies = new List<Currency>();
            foreach (JToken t in token["quote"].Children().ToArray())
            {
                JToken currencyToken = t.Children().First<JToken>();
                Currency currency = new Currency();
                currency.name = currencyToken.ToString().Split(':')[0].Replace("\"", String.Empty);
                currency.price = Convert.ToDecimal(currencyToken["price"]);
                currency.volume_24h = Convert.ToDecimal(currencyToken["volume_24h"]);
                currency.market_cap = Convert.ToDecimal(currencyToken["market_cap"]);
                currency.fully_diluted_market_cap = Convert.ToDecimal(currencyToken["fully_diluted_market_cap"]);
                currencies.Add(currency);
            }
            quote = currencies.ToArray();
        }
        public static async Task<Asset> FromIdAsync(string id)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://cryptingup.com/api/assets/{id}");
            Task<HttpResponseMessage> task = Task.Run(async () => await httpClient.SendAsync(request));
            HttpResponseMessage response = task.Result;
            JToken token = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new Asset(token["asset"]);
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
