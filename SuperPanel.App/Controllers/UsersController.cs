using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.Abstractions;
using SuperPanel.Engines.Derivates;
using SuperPanel.Models;
using System;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDataService _dataService;

        public UsersController(
            ILogger<UsersController> logger,
            IDataService dataService
            )
        {
            _logger = logger;
            _dataService = dataService;
        }

        public IActionResult Index(int? pageNumber)
        {
            try
            {
                var users = _dataService.GetPageListFromUsers(pageNumber) as PaginatedList<User>;

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
