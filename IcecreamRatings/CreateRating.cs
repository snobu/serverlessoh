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
    public static class CreateRating
    {
        static async Task<bool> Exists(string url)
        {
            using var client = new HttpClient();

            var result = await client.GetAsync(url);
            return result.IsSuccessStatusCode;
        }

        static string GetProductUrl(string id) => $"https://serverlessohapi.azurewebsites.net/api/GetProduct?productId={id}";
        static string GetUserUrl(string id) => $"https://serverlessohapi.azurewebsites.net/api/GetUser?userId={id}";


        [FunctionName("CreateRating")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "Ratings",
                CreateIfNotExists = true,
                PartitionKey = "/UserId",
                ConnectionStringSetting = "ConnectionString")]out RatingModel document,          
            ILogger log)
        {

            document = null;
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CreateRatingRequest>(requestBody);
                        
            var id = Guid.NewGuid().ToString();

            if (Exists(GetProductUrl(data.ProductId)).Result == false)
            {
                log.LogWarning($"Product {data.ProductId} not found");
                return new BadRequestResult();
            }

            if (Exists(GetUserUrl(data.UserId)).Result == false)
            {
                log.LogWarning($"User {data.UserId} not found");
                return new BadRequestResult();
            }

            if (data.Rating < 0 || data.Rating >5)
            {
                log.LogWarning($"User {data.Rating} is not between 0 and 5");
                return new BadRequestResult();
            }

            document = new RatingModel {
                Id = id,
                LocationName = data.LocationName,
                ProductId = data.ProductId,
                Rating = data.Rating,
                Timestamp = DateTime.UtcNow,
                UserId = data.UserId,
                UserNotes = data.UserNotes
            };

            return new OkObjectResult(document);
        }
    }
}
