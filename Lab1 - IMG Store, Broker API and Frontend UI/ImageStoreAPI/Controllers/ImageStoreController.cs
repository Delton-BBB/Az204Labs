using Azure.Storage.Blobs.Models;
using ImageStoreAPI.Models;
using ImageStoreAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace ImageStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageStoreController : ControllerBase
    {
        public IAzureBlobStorageService _azureService;

        public ImageStoreController(IAzureBlobStorageService azureService) {
        
            _azureService = azureService;
        }

        [Route("/")]
        [HttpGet]
        public async Task<ActionResult> Home()
        {
            return Redirect("/imagestore");

        }

        [Route("/strings")]
        [HttpGet]
        public async Task<ActionResult> Strings()
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            //return Ok(Environment.GetEnvironmentVariable("CUSTOMCONNSTR_StorageSAS"));
            return Ok(configuration.GetValue<string>("CUSTOMCONNSTR_:Storage:Blob:ConnectionString"));

        }

        [Route("/imagestore/{containerName}/{blobName}")]
        [HttpGet]
        public async Task<ActionResult> GetImage(string containerName, string blobName) 
        {
            try {
                return Ok(await _azureService.getBlob(containerName, blobName));
            }
            catch (Exception e) { 
                return BadRequest(e.Message);
            }

        }

        [Route("/imagestore")]
        [HttpGet]
        public ActionResult GetAllImages()
        {
            try
            
            {
                return Ok(_azureService.getBlobs());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/imagestoredetailed")]
        [HttpGet]
        public ActionResult GetAllImagesDetailed()
        {
            try
            {
                return Ok(_azureService.getBlobsDetailed());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("/imagestore/create/{containerName}/{blobName}")]
        public async Task<ActionResult> Create(string containerName, string blobName, [FromForm] InputFile content)
        {
            try
            {
                FormFile file = new FormFile(content.fileContent.OpenReadStream(), 0, content.fileContent.Length, blobName, content.fileContent.FileName);

                // add a metadata list ([desc: ""],....) which can be stored at metadata for the blob 

                string isCreated = await _azureService
                    .createBlob(containerName, blobName, file);
                return Ok(containerName + "/" + blobName + " Created = " + isCreated);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("/imagestore/delete/{containerName}/{blobName}")]
        public async Task<ActionResult> Delete(string containerName, string blobName)
        {
            try
            {
                string isDeleted = await _azureService.
                    deleteBlob(containerName, blobName);
                return Ok(containerName + "/" + blobName + " Deleted = " + isDeleted);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
