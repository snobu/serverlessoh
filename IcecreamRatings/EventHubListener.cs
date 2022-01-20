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
    public static void Run(
        [EventHubTrigger("salesevents", Connection = "EventHubConnectionAppSetting")] string myEventHubMessage, 
        [CosmosDB(
                databaseName: "Ratings",
                collectionName: "SalesEvents",
                CreateIfNotExists = true,
                PartitionKey = "/Id",
                ConnectionStringSetting = "ConnectionString")] out CombinedJsonRequest document,
        ILogger log)
    {

      log.LogInformation("C# Event Hubs trigger function processed a request.");
      log.LogWarning(myEventHubMessage);

      document = JsonConvert.DeserializeObject<CombinedJsonRequest>(myEventHubMessage);
      document.Id = Guid.NewGuid().ToString();

      log.LogInformation($"Processed events");

    }
  }
}
