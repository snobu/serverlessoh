using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Company.Function
{
    [JsonObject(MemberSerialization.OptIn)]    
    public class BatchEntity 
    {
        [JsonProperty("files")]
        public List<string> Files { get; set; } = new List<string>();
        public void AddFile(string filename)
        {
            Files.Add(filename);
            if (Files.Count == 3)
            {
                // Todo: notify Serverless OpenHack API System
            }
        }

        [FunctionName(nameof(BatchEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
         => ctx.DispatchAsync<BatchEntity>();
    }
}