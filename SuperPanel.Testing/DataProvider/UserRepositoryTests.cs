using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Options;
using SuperPanel.Config;
using Microsoft.Extensions.Logging;
using SuperPanel.DataProvider;
using SuperPanel.Models;

namespace SuperPanel.Testing.DataProvider
{
    public class UserRepositoryTests
    {
        protected Mock<ILogger<UserRepository>> _loggerMock;
        protected Mock<IOptions<DataOptions>> _optionsMock;
        protected UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<UserRepository>>();
            _optionsMock = new Mock<IOptions<DataOptions>>();
            _userRepository = new UserRepository(
                _loggerMock.Object,
                _optionsMock.Object
                );
        }

        [Test]
        public void QueryAll_Success_Return_Empty()
        {
            var list = new List<User>();
            var dataOptions = new DataOptions
            {
                JsonFilePath = string.Empty
            };
            _optionsMock.SetupGet(x => x.Value).Returns(dataOptions);

            var result = _userRepository.QueryAll();

            Assert.AreEqual(list, result);
        }
        [Test]
        public void QueryAll_Success_Return_Count()
        {
            var dataOptions = new DataOptions
            {
                JsonFilePath = "./../../../../data/users.json"
            };
            _optionsMock.SetupGet(x => x.Value).Returns(dataOptions);

            var result = _userRepository.QueryAll();

            Assert.AreEqual(5000, result.Count);
        }


    }
}
