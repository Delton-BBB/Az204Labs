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


        public async Task<bool> createBlob(string containerName, string blobName, FormFile stream)
        {
            bool isUploaded = false;
            FormFile content;

            try
            {
                await _blobServiceClient.GetBlobContainerClient(containerName).CreateIfNotExistsAsync();


                content = new FormFile(stream.OpenReadStream(), 0, stream.Length, blobName, stream.FileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/" + stream.FileName.Split('.').Last()
                };


                BlobClient blob = _blobServiceClient.GetBlobContainerClient(blobName).GetBlobClient(blobName);
                BlobContentInfo response = await blob.UploadAsync(content.OpenReadStream());

                if (response != null)
                {
                    isUploaded = true;

                }
            }
            catch (Exception e)
            {

            }





            return isUploaded;
        }

        public async Task<bool> deleteBlob(string containerName, string blobName)
        {
            bool isDeleted = await _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .DeleteIfExistsAsync();

            return isDeleted;
        }

        public async Task<BlobDownloadResult> getBlob(string containerName, string blobName)
        {
            BlobDownloadResult blobContent = await _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .DownloadContentAsync();

            return blobContent;
        }

        public List<BlobItem> getBlobs()
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
    }
}
