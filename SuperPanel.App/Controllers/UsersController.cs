using ExternalResource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using System.Threading.Tasks;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IClient client;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository, IClient client)
        {
            _logger = logger;
            _userRepository = userRepository;
            this.client = client;
        }

        public IActionResult Index()
        {
            var users = _userRepository.QueryAll();
            return View(users);
        }


        [HttpPost]
        public async Task GDPR(long id)
        {

            //await Task.Delay(500);
            //throw new System.Exception("Test Message");
            var result = await client.GdprAsync(id);
        }

    }
}
