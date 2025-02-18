using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using ImageStoreAPI.Service;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Custom Services
builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();

// Azure Client
builder.Services.AddAzureClients(azure =>
{
    azure.AddBlobServiceClient(builder.Configuration.GetValue<string>("CUSTOMCONNSTR_:Storage:Blob:ConnectionString"));
});

// Logging
builder.Services.AddLogging(builder => builder.AddConsole());

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
