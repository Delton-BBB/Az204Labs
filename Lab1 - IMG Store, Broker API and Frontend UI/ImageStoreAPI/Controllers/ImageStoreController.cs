using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageStoreController : ControllerBase
    {
        [Route("/imagestore")]
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("hello");
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
