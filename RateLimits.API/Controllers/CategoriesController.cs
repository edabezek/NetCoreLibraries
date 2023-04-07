using Microsoft.AspNetCore.Mvc;

namespace RateLimits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        [HttpGet]
        public IActionResult GetCategory()
        {
            return Ok(new { Id = 1, Category = "Kırtasiye" });
        }
    }
}
