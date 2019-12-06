using System;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzStg12Function2
{
    public static class UploadBlobTimerFunction
    {
        static UploadBlobTimerFunction()
        {
            ApplicationHelper.Startup();
        }

        [FunctionName("UploadBlobTimerFunction")]
        public static void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var blobServiceClient = new BlobServiceClient(connectionString: connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName: "files");

            containerClient.CreateIfNotExists();

            var time = DateTime.Now.ToString();
            var blobClient = containerClient.GetBlobClient(blobName: $"{time}.txt");

            blobClient.Upload(content: new MemoryStream(Encoding.UTF8.GetBytes($"Test blob created at {time}"), 0, 12));
        }
    }
}
