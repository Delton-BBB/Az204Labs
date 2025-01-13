using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace ImageStoreAPI.Service
{
    public interface IAzureBlobStorageService
    {
        //public Task<BlobItem> Test();
        public List<BlobItem> getBlobs();
        public Task<BlobDownloadResult> getBlob(string containerName, string blobName);
        public Task<bool> createBlob(string containerName, string blobName, FormFile content);
        public Task<bool> deleteBlob(string containerName, string blobName);

    }
}
