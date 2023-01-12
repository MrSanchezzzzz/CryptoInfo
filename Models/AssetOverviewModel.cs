using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoInfo.Models
{
    internal class AssetOverviewModel
    {
        public class AssetOverview 
        {
            string asset_id;
            string name;

            public AssetOverview(JToken token)
            {
                this.asset_id = token["asset_id"].ToString();
                this.name = token["name"].ToString();
            }
        }
        public async Task<AssetOverview[]> GetAssetOverviews()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://cryptingup.com/api/assetsoverview");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            Console.Error.WriteLine(response.StatusCode.ToString());

            List<AssetOverview> result = new List<AssetOverview>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<JToken> assets = JObject.Parse(await response.Content.ReadAsStringAsync())["assets"].Children().ToList();

                assets.ForEach((element) =>
                {
                    result.Add(new AssetOverview(element));
                });
            }
            else
                Console.Error.WriteLine("Unable to get the contents of responce!");
            return result.ToArray();
        }

    }
}
