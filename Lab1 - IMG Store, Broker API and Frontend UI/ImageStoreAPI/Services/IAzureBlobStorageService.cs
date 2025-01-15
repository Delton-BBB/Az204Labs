using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace ImageStoreAPI.Service
{
    public interface IAzureBlobStorageService
    {
        //public Task<BlobItem> Test();
        public List<BlobItem> getBlobsDetailed();
        public List<string> getBlobs();
        public Task<BlobDownloadResult> getBlob(string containerName, string blobName);
        public Task<string> createBlob(string containerName, string blobName, FormFile content);
        public Task<string> deleteBlob(string containerName, string blobName);


    }
}
