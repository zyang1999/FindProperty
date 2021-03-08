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

        public ActionResult CreateBlobContainer(string blobRef)
        {
            //refer to the container
            CloudBlobContainer container = getBlobStorageInformation(blobRef);
            ViewBag.Success = container.CreateIfNotExistsAsync().Result;
            ViewBag.BlobContainerName = container.Name;
            return View();
        }

        public string uploadBlob(List<IFormFile> images)
        {

            CloudBlobContainer container = getBlobStorageInformation(Guid.NewGuid().ToString());
            container.CreateIfNotExistsAsync().Wait();
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            foreach (var image in images)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
                using (var fileStream = image.OpenReadStream())
                {
                    blob.UploadFromStreamAsync(fileStream).Wait();
                }
            }

            return container.Uri.ToString();
        }


    }
}
