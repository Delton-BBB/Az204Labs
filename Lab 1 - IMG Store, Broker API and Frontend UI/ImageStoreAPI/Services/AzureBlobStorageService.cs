using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ImageStoreAPI.Service
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        public BlobServiceClient _blobServiceClient;
        public ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, ILogger<AzureBlobStorageService> loggerFactory) {
            _blobServiceClient = blobServiceClient;
            _logger = loggerFactory;
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
                _logger.LogError(e, "Error in Create Blob Method");
                throw new Exception(e.Message);
            }
            return isUploaded;
        }

        public async Task<string> deleteBlob(string containerName, string blobName)
        {
            try
            {
                bool isDeleted = await _blobServiceClient
                    .GetBlobContainerClient(containerName)
                    .GetBlobClient(blobName)
                    .DeleteIfExistsAsync();
                return isDeleted.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Delete Blob Method");
                throw new Exception(e.Message);
            }

        }

        public async Task<BlobDownloadResult?> getBlob(string containerName, string blobName)
        {
            try
            {
                BlobDownloadResult blobContent = await _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .DownloadContentAsync();
                return blobContent;
            }
            catch (Exception e){
                _logger.LogError(e, "error in getblob method");
                throw new Exception(e.Message);
            }

        }

        public List<BlobItem>? getBlobsDetailed()
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, "error in Get Blobs Detailed Method");
                throw new Exception(e.Message);
            }
        }

        public List<string>? getBlobs()
        {
            try
            {

                List<string> blobPaths = new List<string>();

                foreach (BlobContainerItem containerItem in _blobServiceClient.GetBlobContainers())
                {
                    foreach (BlobItem blobItem in _blobServiceClient.GetBlobContainerClient(containerItem.Name).GetBlobs())
                    {
                        blobPaths.Add(containerItem.Name + "/" + blobItem.Name);
                    }
                }

                return blobPaths;

            }catch(Exception e)
            {
                _logger.LogError(e, "error in Get Blob Method");
                throw new Exception(e.Message);
            }
        }
    }
}
