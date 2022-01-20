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
                PartitionKey = "/UserId",
                ConnectionStringSetting = "ConnectionString")]out CombinedJson document,          
            ILogger log)
        {

            document = null;
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CreateRatingRequest>(requestBody);
                        
            var id = Guid.NewGuid().ToString();

            document = new CombinedJson {
                Id = id,
    
            };

            return new OkObjectResult(document);
        }
    }
}
