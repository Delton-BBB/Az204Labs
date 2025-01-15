using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using static System.Net.WebRequestMethods;

namespace ImageStoreAPI.Service
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        public BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient) {
            _blobServiceClient = blobServiceClient;
        }


        public async Task<string> createBlob(string containerName, string blobName, FormFile file)
        {
            string isUploaded = "false";
            string fileExtension = file.FileName.Split('.').Last();
            try
            {
                if ((await _blobServiceClient.GetBlobContainerClient(containerName).CreateIfNotExistsAsync()) == null)
                {
                    Console.WriteLine(containerName + " Exists");
                }
                else
                {
                    Console.WriteLine(containerName + " Created");
                }




                BlobClient blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName+"."+fileExtension);
                BlobContentInfo response = await blob.UploadAsync(file.OpenReadStream(),new BlobHttpHeaders() { 
                    ContentType = "image/" + fileExtension
                });

                if (response != null)
                {
                    isUploaded = "true";
                }
            }
            catch (Exception e) {
                return e.Message;
            }
            return isUploaded;
        }

        public async Task<string> deleteBlob(string containerName, string blobName)
        {
            bool isDeleted = await _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .DeleteIfExistsAsync();

            return isDeleted.ToString();
        }

        public async Task<BlobDownloadResult> getBlob(string containerName, string blobName)
        {
            BlobDownloadResult blobContent = await _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .DownloadContentAsync();

            return blobContent;
        }

        public List<BlobItem> getBlobsDetailed()
        {
            List<BlobItem> blobs = [];
            List<BlobContainerItem> containers = new List<BlobContainerItem>();

            foreach (BlobContainerItem containerItem in _blobServiceClient.GetBlobContainers())
            {
                containers.Add(containerItem);
                
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerItem.Name);
                foreach (BlobItem blob in containerClient.GetBlobs())
                {
                    blobs.Add(blob);
                }
            }

            return blobs;
        }

        public List<string> getBlobs()
        {
            List<string> blobPaths = new List<string>();

            foreach (BlobContainerItem containerItem in _blobServiceClient.GetBlobContainers())
            {
                foreach (BlobItem blobItem in _blobServiceClient.GetBlobContainerClient(containerItem.Name).GetBlobs())
                {
                    blobPaths.Add(containerItem.Name+"/"+blobItem.Name);
                }
            }

            return blobPaths;
        }


    }
}
