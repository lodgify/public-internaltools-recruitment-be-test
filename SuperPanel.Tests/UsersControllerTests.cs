using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NToastNotify;
using SuperPanel.App.Controllers;
using SuperPanel.App.Data;
using SuperPanel.App.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SuperPanel.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _sut;
        private readonly Mock<ILogger<UsersController>> _loggerMock = new Mock<ILogger<UsersController>>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly UserRepository user_repo = new UserRepository(Options.Create<DataOptions>(new DataOptions()
        {
            JsonFilePath = "./../../../../data/users.json"
        }));
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<IToastNotification> _toastNotificationMock = new Mock<IToastNotification>();
   

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
            Assert.Equal(50,model.Data.Count());
        }

        /// <summary>
        /// Method used to mock a HTTPClient object by mocking the HTTPMessageHandler()
        /// </summary>
        /// <returns>
        /// Controller with mocked IHttpClientFactory
        /// </returns>
        public UsersController HTTPClientMock()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'name':SagradaFamilia,'city':'BCN'}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            UsersController controller = new UsersController(_loggerMock.Object, _userRepositoryMock.Object, mockFactory.Object, _toastNotificationMock.Object);
            return controller;
        }

        [Fact]

        public async Task DeleteConfirmed_ShouldRedirect_toIndexOrDelete_View_WhenHTTPConnectionPossible()
        {
            //Arrange
            var id = 10203;
            string[] acceptedActions = { "Index", "Delete" };
            var controller = HTTPClientMock();

            //Act
            var result = await controller.DeleteConfirmed(id);

            //Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(string.Equals(redirectToActionResult.ActionName, "Index") || string.Equals(redirectToActionResult.ActionName, "Delete"));
                            
        }

    }
}
