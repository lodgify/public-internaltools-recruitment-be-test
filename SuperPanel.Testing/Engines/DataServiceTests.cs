using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperPanel.Config;
using SuperPanel.Abstractions;
using SuperPanel.Engines.Services;
using SuperPanel.Models;
using SuperPanel.Engines.Derivates;

namespace SuperPanel.Testing.Engines
{
    public class DataServiceTests
    {
        protected Mock<ILogger<DataService>> _loggerMock;
        protected Mock<IOptions<DataOptions>> _optionsMock;
        protected Mock<IUserRepository> _userMock;
        protected DataService _dataService;
        protected PaginatedList<User> _paginatedList;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<DataService>>();
            _optionsMock = new Mock<IOptions<DataOptions>>();
            _userMock = new Mock<IUserRepository>();

            _dataService = new DataService(
                _loggerMock.Object,
                _optionsMock.Object,
                _userMock.Object
                );
        }

        [Test]
        public void GetPageListFromUsers_Success()
        {
            var list = new List<User>(){ new User{ Id = 1 } };
            var dataOptions = new DataOptions
            {
                PageSize = 1
            };
            _userMock.Setup(x => x.QueryAll()).Returns(list);
            _optionsMock.SetupGet(x => x.Value).Returns(dataOptions);

            var result = _dataService.GetPageListFromUsers(It.IsAny<int>());

            Assert.AreEqual(list, result);
        }
    }
}
