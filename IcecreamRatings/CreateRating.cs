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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateRatingRequest>(requestBody);
                        
            var id = Guid.NewGuid().ToString();

            if (await Exists(GetProductUrl(data.ProductId)) == false)
            {
                log.LogWarning($"Product {data.ProductId} not found");
                return new NotFoundResult();
            }

            if (await Exists(GetUserUrl(data.UserId)) == false)
            {
                log.LogWarning($"User {data.UserId} not found");
                return new NotFoundResult();
            }
            
            // persist in cosmos

            // return the model with an ID

            return new OkObjectResult(data);
        }
    }
}
