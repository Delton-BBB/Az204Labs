using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using ImageStoreAPI.Service;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//string connectionString = "BlobEndpoint=https://deltonstorageaccount.blob.core.windows.net/;QueueEndpoint=https://deltonstorageaccount.queue.core.windows.net/;FileEndpoint=https://deltonstorageaccount.file.core.windows.net/;TableEndpoint=https://deltonstorageaccount.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2026-01-12T21:52:28Z&st=2025-01-12T13:52:28Z&spr=https&sig=XAP8sQPtTPrMIh7WaNQfgAIhP3UfYp2JsNxZ8ipxpjg%3D";


builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddAzureClients(azure =>
{
    azure.AddBlobServiceClient(builder.Configuration.GetValue<string>("Azure:Storage:Blob:ConnectionString"));
});

//builder.Services.AddSingleton(x =>
//{
//    return new BlobServiceClient(connectionString);
//});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
