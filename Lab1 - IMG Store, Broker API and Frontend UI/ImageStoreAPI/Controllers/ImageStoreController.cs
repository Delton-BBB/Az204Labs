using ImageStoreAPI.Models;
using ImageStoreAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [Route("/imagestore/{containerName}/{blobName}")]
        [HttpGet]
        public async Task<ActionResult> GetImage(string containerName, string blobName) 
        {
            return Ok(await _azureService.getBlob(containerName, blobName));
        }

        [Route("/imagestore")]
        [HttpGet]
        public ActionResult GetAllImages()
        {
           return Ok(_azureService.getBlobs());
        }

        [HttpPost]
        [Route("/imagestore/create/{containerName}/{blobName}")]
        public async Task<ActionResult> Create(string containerName, string blobName, [FromForm] InputFile content)
        {
            FormFile file = new FormFile(content.fileContent.OpenReadStream(), 0, content.fileContent.Length, blobName, content.fileContent.FileName);
           

            bool isCreated = await _azureService
                .createBlob(containerName, blobName, file);
            return Ok(containerName + "/" + blobName + " Created");
        }

        [HttpPost]
        [Route("/imagestore/delete/{containerName}/{blobName}")]
        public async Task<ActionResult> Delete(string containerName, string blobName)
        {
            bool isDeleted = await _azureService.
                deleteBlob(containerName, blobName);
            return Ok(containerName +"/" +blobName + " Deleted");
        }

    }
}
