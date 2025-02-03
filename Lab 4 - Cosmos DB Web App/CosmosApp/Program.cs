using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add cosmos client to the container
builder.Services.AddSingleton(c =>
{
    return new CosmosClient(builder.Configuration["ConnectionStrings:CosmosDB"]);
});

// Add Azure clients to the container
builder.Services.AddSingleton(c =>
{
    return new BlobServiceClient(builder.Configuration["ConnectionStrings:StorageAccount"]);
});


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
