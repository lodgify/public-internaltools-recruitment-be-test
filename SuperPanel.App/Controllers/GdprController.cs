using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.Abstractions;
using SuperPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Controllers
{
    public class GdprController : Controller
    {
        private readonly IGdprService _gdprService;
        private readonly ILogger<GdprController> _logger;
        public GdprController(
            IGdprService gdprService,
            ILogger<GdprController> logger
            )
        {
            _logger = logger;
            _gdprService = gdprService;
        }
        
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var user = await _gdprService.SelectUser(id);
                if(user == null)
                {
                    ModelState.AddModelError("Error", "There are some problems getting the user. Please, try again!");
                }
                else if(user.Id < 0)
                {
                    ModelState.AddModelError("Error", "This user is not present. Please, select another one");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
 
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var userDeleted = await _gdprService.DeleteUser(id);
                if (userDeleted == null)
                {
                    ModelState.AddModelError("Error", "There are some problems deleting the user. Please, try again!");
                }

                return View(userDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public ActionResult DeleteUsers()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(string usersList)
        {
            try
            {
                var list = usersList.Split("\r\n").ToList();
                var usersDeleted = new List<User>();
                foreach (var item in list)
                {
                    var userDeleted = await _gdprService.DeleteUser(item);
                    if (userDeleted != null)
                    {
                        usersDeleted.Add(userDeleted);
                    }
                }

                return View("DeleteUsersReport", usersDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }            
        }

    }
}
