using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NToastNotify;
using SuperPanel.App.Controllers;
using SuperPanel.App.Data;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SuperPanel.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _sut;
        private readonly Mock<ILogger<UsersController>> _loggerMock = new Mock<ILogger<UsersController>>();
        private readonly Mock<IOptions<DataOptions>> dataOptions = new Mock<IOptions<DataOptions>>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly UserRepository user_repo = new UserRepository(Options.Create<DataOptions>(new DataOptions()
        {
            JsonFilePath = "./../../../../data/users.json"
        }));
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<IToastNotification> _toastNotificationMock = new Mock<IToastNotification>();
        private readonly Random _random;

        public UsersControllerTests()
        {
            _sut = new UsersController(_loggerMock.Object, user_repo, _httpClientFactoryMock.Object, _toastNotificationMock.Object);
        }

        [Fact]
        public void Index_ShouldReturnViewResult_WithFilteredUsers()
        {
            //Arrange

            var pageNumber = 2;
            var pageSize = 50;
            var excluded_files = (pageSize * pageNumber) - pageSize;

            //Act
            var view_result = _sut.Index(It.IsAny<int>(), pageNumber, pageSize) as ViewResult;

            //Assert
            Assert.IsType<ViewResult>(view_result);
            var model = Assert.IsAssignableFrom<PagedResult<App.Models.User>>(
        view_result.ViewData.Model);
            Assert.Equal(model.TotalItems - excluded_files,model.Data.Count());
        }


        [Fact]

        public async Task<IActionResult> DeleteConfirmed_ShouldRedirect_toIndex_WhenNotPossibleToDelete()
        {

        }

        //[Fact]
        //public async Task<IActionResult> DeleteConfirmed_ShouldRedirect_toDelete_WhenInternalErrorOccurs()
        //{

        //}
    }
}
