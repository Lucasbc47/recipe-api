using Microsoft.AspNetCore.Mvc;

namespace Recipe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult HandleError()
        {
            return Problem(
                title: "An error occurred while processing your request.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
} 