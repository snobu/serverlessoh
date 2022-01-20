using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using IcecreamRatings.Models;
using System.Linq;

namespace IcecreamRatings
{
  public static class EventHubListener
  {


    [FunctionName("EventHubListener")]
    public static async Task Run(
        [EventHubTrigger("team9evenhubworkspace", Connection = "EventHubConnectionAppSetting")] string myEventHubMessage, 
        [CosmosDB(
                databaseName: "Ratings",
                collectionName: "CombinedJson",
                CreateIfNotExists = true,
                PartitionKey = "/Id",
                ConnectionStringSetting = "ConnectionString")] IAsyncCollector<CombinedJsonRequest> documents,
        ILogger log)
    {

      log.LogInformation("C# Event Hubs trigger function processed a request.");
      log.LogWarning(myEventHubMessage);

      var data = JsonConvert.DeserializeObject<CombinedJsonRequest[]>(myEventHubMessage);

      foreach (var item in data)
      {
          item.Id = Guid.NewGuid().ToString();
          await documents.AddAsync(item);
      }

    }
  }
}
