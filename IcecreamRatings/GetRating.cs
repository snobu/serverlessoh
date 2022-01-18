using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IcecreamRatings
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "Ratings",
                ConnectionStringSetting = "ConnectionString",
                Id = "{Query.ratingId}",
                PartitionKey = "{Query.userId}"
            )]Models.RatingModel rating,
            ILogger log)
        {
            log.LogInformation("Get rating");
  
            if (null == rating)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(rating);
        }
    }
}
