using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using cloudscribe.Pagination.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using SuperPanel.App.Models;
using System;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
 
        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository,IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
        }

        //public IActionResult Index2()
        //{
        //    var users = _userRepository.QueryAll();
        //    return View(users);
        //}
        public IActionResult Index(int pageNumber=1,int pageSize=50)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var number_users = _userRepository.Get_Number_Users();
            var filtered_users = _userRepository.QueryAll().Skip(excludeRecords).Take(pageSize);
            
            var result = new PagedResult<Models.User>
            {
                Data = filtered_users.ToList(),
                TotalItems = number_users,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        public IActionResult Delete(int id)
        {
            var user = _userRepository.QueryUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var URL = $"http://localhost:61695/v1/contacts/";
            var user = new User(id);
            var message = "";

            try
            {
                //Check if user exists in External Contacts API
                var response = await httpClient.GetAsync($"{URL}{id}");

                //If user exists, attempt to Anonymize it from Contact List.
                if (response.IsSuccessStatusCode)
                {
                    var Grdp_response = await httpClient.PutAsJsonAsync($"{URL}{id}/gdpr", user);
                    if(Grdp_response.IsSuccessStatusCode)
                    {
                        message = "Sucessfully deleted User";
                        _userRepository.Remove_User(user);
                    }
                    else
                    {
                        message = $"Error while deleting User.Please contact Support.";
                    }
                }
                else
                {
                    message = $"Error while deleting User with Id.Please contact Support.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
   
            return RedirectToAction(nameof(Index));
        }
    }
}
