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
    public static class WriteCombinedJson
    {


        [FunctionName("WriteCombinedJson")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "CombinedJson",
                CreateIfNotExists = true,
                PartitionKey = "/Id",
                ConnectionStringSetting = "ConnectionString")]out CombinedJsonModel document,          
            ILogger log)
        {

            document = null;
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CombinedJsonRequest[]>(requestBody);
                        
            document.Id = Guid.NewGuid().ToString();
            document.CombinedJsonRequest = data.ToList();

            return new OkObjectResult(document);
        }
    }
}
