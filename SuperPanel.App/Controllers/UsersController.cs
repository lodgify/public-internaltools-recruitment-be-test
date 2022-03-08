using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.Abstractions;
using System;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;

        public UsersController(
            ILogger<UsersController> logger,
            IUserRepository userRepository
            )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var users = _userRepository.QueryAll();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems rendering Index: {ex.StackTrace}");
                throw;
            }
        }


    }
}
