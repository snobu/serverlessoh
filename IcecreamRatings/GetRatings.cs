using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IcecreamRatings
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetRatings/{userId}")] HttpRequest req,
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "Ratings",
                ConnectionStringSetting = "ConnectionString",
                PartitionKey = "{userId}",
                SqlQuery = "select * from r where r.UserId = {userId}"
            )]IEnumerable<Models.RatingModel> ratings,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(ratings);
        }
    }
}
