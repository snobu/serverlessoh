using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Company.Function
{
    public static class BatchFileProcessing
    {
        // Todo: Add EventGrid to monitor storage Account
        [FunctionName("BatchFileProcessing_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableClient starter,
            ILogger log)
        {
            string file = req.Query["batchFile"];
            var id = file.Split('-')[0];
            var batchEntity = new EntityId(nameof(BatchEntity), id);

            await starter.SignalEntityAsync(batchEntity, "AddFile", file);
            return new OkObjectResult(file);
        }

        [FunctionName("ProcessBlob")]
        public static async Task Run(
            [EventGridTrigger] EventGridEvent evt,
            [DurableClient] IDurableClient starter,
            ILogger log)
        {
            if (evt.TryGetSystemEventData(out object eventData))
            {
                if (eventData is StorageBlobCreatedEventData)
                {
                    var blobCreatedEvent = eventData as StorageBlobCreatedEventData;
                    var uri = new Uri(blobCreatedEvent.Url);   
                    
                    var filename = System.IO.Path.GetFileName(uri.LocalPath);
                    var id = filename.Split('-')[0];
                    var batchEntity = new EntityId(nameof(BatchEntity), id);

                    await starter.SignalEntityAsync(batchEntity, "AddFile", blobCreatedEvent.Url);
                    return;
                }
            }
        }
    }
}