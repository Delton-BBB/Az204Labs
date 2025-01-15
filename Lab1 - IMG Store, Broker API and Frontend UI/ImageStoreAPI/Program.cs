using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using ImageStoreAPI.Service;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddAzureClients(azure =>
{
    azure.AddBlobServiceClient(builder.Configuration.GetValue<string>("CUSTOMCONNSTR_:Storage:Blob:ConnectionString"));
});
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
