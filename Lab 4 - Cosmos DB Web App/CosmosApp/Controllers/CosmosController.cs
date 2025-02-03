using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;


namespace CosmosApp.Controllers
{
    [ApiController]
    [Route("Cosmos")]
    public class CosmosController : Controller
    {
        public CosmosClient _cosmosClient;
        public Container _container;

        public CosmosController(CosmosClient cosmosClient, BlobServiceClient blobClient)
        {
            _container = cosmosClient.GetContainer("Retail", "Online");
            _cosmosClient = cosmosClient;
        }

        [Route("/{id}/{pkey}")]
        public async Task<IActionResult> Index(string id, string pkey)
        {

            // try to retrive one value
            ResponseMessage item = await _container.ReadItemStreamAsync(id, new PartitionKey(pkey));
            if (item.Content != null)
            {
                //using (StreamReader sr = new StreamReader(item.Content))
                //{
                //    Console.WriteLine(await sr.ReadToEndAsync());
                //}
                return Ok(item.Content);
            }
            else
            {
                return Ok(item.StatusCode);
            }
        }

        [Route("CreateContainer")]
        public async Task<IActionResult> CreateContainer()
        {
            // Create if DB and Container do not exist 
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Retail");
            Container container = await database.CreateContainerIfNotExistsAsync("Online", "/Category");

            return View();
        }

        [Route("GetAllItems")]
        public async Task<IActionResult> GetAllItemsUsingSQlQuery()
        {
            // Try to retrive all values using sql query 
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryStreamIterator(query);
            while (iterator.HasMoreResults)
            {
                using (ResponseMessage response = await iterator.ReadNextAsync())
                {
                    using (StreamReader sr = new StreamReader(response.Content))
                    {
                        Console.WriteLine(await sr.ReadToEndAsync());
                    }
                }
            }

            return View();
        }
    }
}
