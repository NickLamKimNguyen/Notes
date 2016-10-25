using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Utils
{
    public class AzureStorageUtil
    {
        public static async Task<string> UploadToAzureStorage(string connectionString, string containerName, string fileName, byte[] data)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            var blobContainer = blobClient.GetContainerReference(containerName);
            var blob = blobContainer.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(data, 0, data.Length);
            return blob.Uri.ToString();
        }
    }
}
