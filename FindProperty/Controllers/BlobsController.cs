using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Controllers
{
    public class BlobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public CloudBlobContainer getBlobStorageInformation(string blobRef)
        {
            //step 1: read json
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();

            //to get key access
            //once link, time to read the content to get the connectionstring
            CloudStorageAccount objectaccount = CloudStorageAccount.Parse(configure["ConnectionStrings:BlobStorageConnection"]);
            CloudBlobClient blobclient = objectaccount.CreateCloudBlobClient();
            //step 2: how to create a new container in the blob storage account.
            CloudBlobContainer container =
           blobclient.GetContainerReference(blobRef);

            return container;
        }

        public CloudBlobContainer CreateBlobContainer()
        {
            //refer to the container
            var blobRef = Guid.NewGuid().ToString();
            CloudBlobContainer container = getBlobStorageInformation(blobRef);
            container.CreateIfNotExistsAsync().Wait();
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container;
        }
        public void createBlockBlob(CloudBlobContainer container, IFormFile image)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
            using (var fileStream = image.OpenReadStream())
            {
                blob.UploadFromStreamAsync(fileStream).Wait();
            }
        }

        public IEnumerable<IListBlobItem> getBlockBlobs(string containerName)
        {
            return getBlobStorageInformation(containerName).ListBlobsSegmentedAsync(null).Result.Results;
        }

        public string uploadBlobs(List<IFormFile> images)
        {          
            CloudBlobContainer container = CreateBlobContainer();
            foreach (var image in images)
            {
                createBlockBlob(container, image);
            }
            return container.Name;
        }

        public string uploadBlob(IFormFile image)
        {
            CloudBlobContainer container = CreateBlobContainer();
            createBlockBlob(container, image);
            return container.Name;
        }

        public string editBlob(string blobRef, List<IFormFile> images)
        {
            deleteBlob(blobRef);
            return uploadBlobs(images);
        }

        public void deleteBlob(string blobRef)
        {
            CloudBlobContainer container = getBlobStorageInformation(blobRef);
            container.DeleteIfExistsAsync().Wait();
        }

    }
}
