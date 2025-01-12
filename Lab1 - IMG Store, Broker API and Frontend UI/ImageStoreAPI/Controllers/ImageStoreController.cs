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



        [Route("/imagestore")]
        [HttpGet]
        public ActionResult GetAllImages()
        {
           


           return Ok(_azureService.getBlobs());
        }

        [HttpGet]
        [Route("/imagestore/create/{name}")]
        public ActionResult Create(string name)
        {
            return Ok(name + "Created");
        }

        [HttpGet]
        [Route("/imagestore/delete/{name}")]
        public ActionResult Delete(string name)
        {
            return Ok(name + "Deleted");
        }

    }
}
