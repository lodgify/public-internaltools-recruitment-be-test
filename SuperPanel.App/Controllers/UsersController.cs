using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Data;
using cloudscribe.Pagination.Models;
using System.Linq;

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

    }
}
