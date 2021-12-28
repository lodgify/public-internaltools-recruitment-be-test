using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using cloudscribe.Pagination.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using SuperPanel.App.Models;
using System;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IToastNotification _toastNotification;
        private static bool startup = true;
        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository,IHttpClientFactory httpClientFactory, IToastNotification toastNotification)
        {
            _logger = logger;
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
            _toastNotification = toastNotification;
        }

        //TODO-Delete deprecated code
        //public IActionResult Index2()
        //{
        //    var users = _userRepository.QueryAll();
        //    return View(users);
        //}
        public IActionResult Index(int id, int pageNumber = 1, int pageSize = 50)
        {
            if (startup) _toastNotification.RemoveAll();
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var result = _userRepository.Get_FilteredUsers(excludeRecords, pageNumber, pageSize);
      
            startup = false;
            return View(result);
        }

        /// <summary>
        /// Queries the user repo the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// View of the user returned by the id
        /// </returns>
        public IActionResult Delete(int id)
        {
            var user = _userRepository.QueryUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// //If the Status Code is 500, either by attempting the GetAsync() or the PutAsJsonAsync(), an internal error occured
            //due to the instability of the ExternalContactsAPI. Warn the user and suggest an new attempt
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// IActionResult. Either Index or Delete 
        /// </returns>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var URL = $"http://localhost:61695/v1/contacts/";
            var user = new User(id);
            string redirected_action = "Index";
            try
            {
                //Check if user exists in External Contacts API
                var response = await httpClient.GetAsync($"{URL}{id}");
            
                switch((int)response.StatusCode)
                {
                    case 404:
                        _toastNotification.AddErrorToastMessage(OutputMessages.user_not_found);
                        _logger.LogError(OutputMessages.user_not_found);
                        break;         
                    case 500:
                        redirected_action = "Delete";
                        _toastNotification.AddWarningToastMessage(OutputMessages.alert_message);
                        _logger.LogWarning(OutputMessages.alert_message);
                        break;
                    case 200:
                        //Attempt to delete from Contact List
                        var Grdp_response = await httpClient.PutAsJsonAsync($"{URL}{id}/gdpr", user);
                        if((int)Grdp_response.StatusCode == 200)
                        {
                            _userRepository.Remove_User(user);
                            _toastNotification.AddSuccessToastMessage(OutputMessages.sucess);
                            _logger.LogInformation(OutputMessages.sucess);
                        }
                        else if((int)Grdp_response.StatusCode == 500)
                        {
                            redirected_action = "Delete";
                            _toastNotification.AddWarningToastMessage(OutputMessages.alert_message);
                            _logger.LogWarning(OutputMessages.alert_message);
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage(OutputMessages.general_error);
                            _logger.LogError(OutputMessages.general_error);
                        }
                        break;
                    default:
                        _toastNotification.AddErrorToastMessage(OutputMessages.general_error);
                        _logger.LogError(OutputMessages.general_error);
                        break;
                }
           
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();              
            }
   
            return RedirectToAction(redirected_action,new {id=id});
        }
    }
}
