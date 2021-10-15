using Microsoft.AspNetCore.Mvc;

namespace ExternalContacts.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/error")]
        public IActionResult Error() 
            => Problem();
    }
}
