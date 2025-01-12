using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace ImageStoreAPI.Service
{
    public interface IAzureBlobStorageService
    {
        //public Task<BlobItem> Test();
        public List<BlobItem> getBlobs();
        public Task<Blob> getBlob(string blobName);
        public Task<Boolean> createBlob(string blobName);
        public Task<Boolean> deleteBlob(string blobName);

    }
}
