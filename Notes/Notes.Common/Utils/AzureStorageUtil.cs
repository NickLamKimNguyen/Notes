using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Notes.Common.Utils
{
    public class AzureStorageUtil
    {
        public static async Task<string> UploadFileToAzureStorage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                using (var mm = new MemoryStream())
                {
                    await file.InputStream.CopyToAsync(mm);
                    return await UploadToAzureStorage("product-images", Guid.NewGuid() + Path.GetExtension(file.FileName), mm.ToArray());
                }
            }
            return string.Empty;
        }

        public static async Task<string> UploadToAzureStorage(string containerName, string fileName, byte[] data)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["FileCloudStorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            var blobContainer = blobClient.GetContainerReference(containerName);
            var blob = blobContainer.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(data, 0, data.Length);
            return blob.Uri.ToString();
        }
    }
}
