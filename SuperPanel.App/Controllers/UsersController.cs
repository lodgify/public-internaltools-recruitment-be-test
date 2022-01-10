using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
using SuperPanel.App.Models;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(
            string currentFilter,
            string searchString,
            int? pageNumber,
            int pageSize = 50,
            string sortOrder = "id_asc")
        {
            dynamic temp = SortResult("Sort");

            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParam"] = 
                String.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["NameSortParam"] = 
                String.IsNullOrEmpty(sortOrder) || sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["EmailSortParam"] =
                String.IsNullOrEmpty(sortOrder) || sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewData["CreatedAtSortParam"] =
                String.IsNullOrEmpty(sortOrder) || sortOrder == "createdAt_asc" ? "createdAt_desc" : "createdAt_asc";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var users = from u in _userRepository.QueryAll() 
                        select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.LastName.Contains(searchString)
                                       || u.FirstName.Contains(searchString));
                temp = SortResult("Search");
            }

            switch (sortOrder)
            {
                case "id_desc":
                    users = users.OrderByDescending(u => u.Id);
                    break;
                case "id_asc":
                    users = users.OrderBy(u => u.Id);
                    break;
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "name_asc":
                    users = users.OrderBy(u => u.LastName);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "email_asc":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "createdAt_desc":
                    users = users.OrderByDescending(u => u.CreatedAt);
                    break;
                case "createdAt_asc":
                    users = users.OrderBy(u => u.CreatedAt);
                    break;
                default:
                    users = users.OrderBy(u => u.Id);
                    break;
            }
            await Task.WhenAll();

            TempData["AlertMessage"] = temp.SortResult;
            TempData["AlertType"] = temp.AlertType;

            return View(await PaginatedList<Models.User>.CreateAsync(users.AsQueryable(), pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Index(
            string currentFilter,
            string searchString,
            int? pageNumber,
            int pageSize,
            IFormCollection form,
            int[] userIds,
            string sortOrder = "id_asc")
        {
            dynamic temp = SortResult("Post");

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var users = from u in _userRepository.QueryAll()
                        select u;

            switch (sortOrder)
            {
                case "id_desc":
                    users = users.OrderByDescending(u => u.Id);
                    break;
                case "id_asc":
                    users = users.OrderBy(u => u.Id);
                    break;
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "name_asc":
                    users = users.OrderBy(u => u.LastName);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "email_asc":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "createdAt_desc":
                    users = users.OrderByDescending(u => u.CreatedAt);
                    break;
                case "createdAt_asc":
                    users = users.OrderBy(u => u.CreatedAt);
                    break;
                default:
                    users = users.OrderBy(u => u.Id);
                    break;
            }
            await Task.WhenAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.LastName.Contains(searchString)
                                       || u.FirstName.Contains(searchString));
                temp = SortResult("Search");
            }

            int n = userIds.Length;
            if ( n > 0 )
            {
                for (int i = 0; i < n; i++)
                {
                    int user = userIds[i];
                    if ( user > 0 ) { 
                    SubmitDelRequest(user);
                    RemoveUser(user);
                    }
                }
            }

            var sessionPageSize = Convert.ToInt32(form["pageSize"]);
            if (sessionPageSize > 0)
            {
                pageSize = Convert.ToInt32(sessionPageSize);
            }
            else
            {
                pageSize = 50;
            }

            return View("Index", await PaginatedList<Models.User>.CreateAsync(users.AsQueryable(), pageNumber ?? 1, pageSize));
        }
        public void SubmitDelRequest(int userId)
        {
            if (userId > 0)
            {
                HttpClient client = new HttpClient();
                string delRequestStr = $@"{{
                  ""status"": 1,
                  ""userId"": {userId},
                  ""priority"": 1,
                  ""processedDate"": ""{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")}""
                }}";

                var delRequest = new StringContent(delRequestStr, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("https://localhost:44363/api/v1/DeletionRequest/");
                HttpResponseMessage response = client.PostAsync("deletionRequests", delRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    // To Do: refactor this
                    TempData["AlertType"] = "alert alert-success";
                    TempData["AlertMessage"] = $"Deletion Request sent for User: {userId}";
                }
                else
                {
                    TempData["AlertType"] = "alert alert-danger";
                    TempData["AlertMessage"] = $"Something went wrong. Please, try again.";
                }
            }
        }
        public IEnumerable<User> RemoveUser(int userId)
        {
            var FilePath = "./../data/users.json";
            var json = System.IO.File.ReadAllText(FilePath);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                            .ToList();
            var index = users.FindIndex(u => u.Id == userId);
            
            if (index > 0 && index < users.Count) { 
            users.RemoveAt(index);
            }
            var userStr = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(FilePath, userStr);
            return users;
        }
        public object SortResult(string action)
        {
            // Terminar: distintos tipos de mensajes según param usado para sort
            var result = $"{action} completed successfully";
            var alert = "alert alert-success";
            return new { SortResult = result, AlertType = alert };
        }
    }
}
