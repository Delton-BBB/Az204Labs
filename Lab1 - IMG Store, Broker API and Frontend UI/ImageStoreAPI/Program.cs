using Azure.Core.Extensions;
using ImageStoreAPI.Service;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddAzureClients(azure =>
{
    azure.AddBlobServiceClient(builder.Configuration.GetRequiredSection("Azure:Storage:Blob"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
