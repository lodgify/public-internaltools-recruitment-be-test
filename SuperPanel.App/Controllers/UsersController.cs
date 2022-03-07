using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using SuperPanel.App.Service;
using System.Threading.Tasks;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGdprService _gdprService;

        public UsersController(ILogger<UsersController> logger, 
            IUserRepository userRepository, IGdprService gdprService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _gdprService = gdprService;
        }

        public IActionResult Index(int? page)
        {
            var users = _userRepository.QueryAll(page??1);
            return View(users);
        }


        public async Task<IActionResult> Gdpr(int page, int id)
        {
            var res = await _gdprService.GdprOrchestrator(id);

            if (!string.IsNullOrEmpty(res))
                TempData["error"] = res;
            
            return RedirectToAction("Index", new { page });
        }
    }
}
