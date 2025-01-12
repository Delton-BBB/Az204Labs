using Azure.Storage.Blobs;

namespace ImageStoreAPI.Service
{
    public interface IAzureBlobStorageService
    {
        public BlobClient GetBlobClient();

        public void getBlobs();
        public void getBlob(string blobName);
        public void createBlob(string blobName);
        public void deleteBlob(string blobName);

    }
}
