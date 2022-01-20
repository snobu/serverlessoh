using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Company.Function
{
    [JsonObject(MemberSerialization.OptIn)]    
    public class BatchEntity 
    {

        async Task<string> CallApi(RequestModel model)
        {
            const string url = "https://serverlessohmanagementapi.trafficmanager.net/api/order/combineOrderContent";

            using var client = new HttpClient();
            
            var result = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "appliction/json"));
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        async Task<bool> CallFuction(string requestData)
        {
            const string url = "https://team9prodfunc.azurewebsites.net/api/WriteCombinedJson?code=fYzjnWoXmmzjExScxaCvPkKu34LSHLEzF6TomVjNpoz3pf/RpE25Tg==&clientId=default";
            using var client = new HttpClient();
            
            var result = await client.PostAsync(url, new StringContent(requestData, System.Text.Encoding.UTF8, "appliction/json"));
            return result.IsSuccessStatusCode;
        }

        [JsonProperty("files")]
        public List<string> Files { get; set; } = new List<string>();
        public void AddFile(string filename)
        {
            Files.Add(filename);
            if (Files.Count == 3)
            {
                var jsonDocument = CallApi(new RequestModel{
                    OrderHeaderDetailsCSVUrl = Files.FirstOrDefault(x => x.Contains("OrderHeaderDetails")),
                    OrderLineItemsCSVUrl = Files.FirstOrDefault(x => x.Contains("OrderLineItems")),
                    ProductInformationCSVUrl = Files.FirstOrDefault(x => x.Contains("ProductInformation"))
                }).Result;

                Console.WriteLine(jsonDocument);

                CallFuction(jsonDocument).Wait();
            }
        }

        [FunctionName(nameof(BatchEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
         => ctx.DispatchAsync<BatchEntity>();
    }
}