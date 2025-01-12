using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;

namespace ImageStoreAPI.Service
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        public BlobServiceClient _blobServiceClient;
        public string _containerName = "imagestore";



        public AzureBlobStorageService(BlobServiceClient blobServiceClient) {
        
            _blobServiceClient = blobServiceClient;
        }


        public async Task<bool> createBlob(string blobName)
        {
            //BlobClient blob = _blobServiceClient.GetBlobContainerClient("").GetBlobClient(blobName);

            //FormFile file = new FormFile(0,0,0,blobName,blobName);
            //StreamContent content = new StreamContent();

            //return await blob.UploadAsync();

            return true;
        }

        public Task<bool> deleteBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public async Task<BlobDownloadResult> getBlob(string blobName)
        {
            BlobClient x =  _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(blobName);
            BlobDownloadResult b = await x.DownloadContentAsync();
            return b;
        }


        Task<Blob> IAzureBlobStorageService.getBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public List<BlobItem> getBlobs()
        {

            string connectionString = "BlobEndpoint=https://deltonstorageaccount.blob.core.windows.net/;QueueEndpoint=https://deltonstorageaccount.queue.core.windows.net/;FileEndpoint=https://deltonstorageaccount.file.core.windows.net/;TableEndpoint=https://deltonstorageaccount.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2026-01-12T21:52:28Z&st=2025-01-12T13:52:28Z&spr=https&sig=XAP8sQPtTPrMIh7WaNQfgAIhP3UfYp2JsNxZ8ipxpjg%3D";
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            //BlobContainerClient containerClient = new BlobServiceClient(connectionString)
            //    .GetBlobContainerClient(_containerName);


            List<BlobItem> blobs = [];

            foreach (BlobItem blob in containerClient.GetBlobs())
            {
                blobs.Add(blob);
            }

            return blobs;
        }
    }
}
