using Azure.Storage.Blobs;

namespace ImageStoreAPI.Service
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        AzureBlobStorageService _storageService;

        AzureBlobStorageService(AzureBlobStorageService storageService)
        {
            _storageService = storageService;
        }

        private BlobServiceClient GetBlobServiceClient()
        {
            return _storageService.GetBlobServiceClient();
        }



        public void createBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public void deleteBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public void getBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public BlobClient GetBlobClient()
        {
            throw new NotImplementedException();
        }

        public void getBlobs()
        {
            throw new NotImplementedException();
        }
    }
}
