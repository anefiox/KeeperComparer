using Microsoft.AspNetCore.Mvc;

namespace KeeperComparer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("API is working!");
        }
    }
}