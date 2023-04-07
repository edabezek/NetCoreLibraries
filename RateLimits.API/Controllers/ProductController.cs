using Microsoft.AspNetCore.Mvc;

namespace RateLimits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(new { Id = 1, Name = "Kırtasiye" , Price=20});
        }
    }
}
