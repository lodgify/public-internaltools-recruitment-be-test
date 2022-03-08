using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperPanel.App.Data;
using SuperPanel.App.Infrastructure;
using System.IO;
using System.Linq;
using Xunit;

namespace SuperPanel.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void QueryAll_ShouldReturnFirstPage()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var r = new UserRepository(Options.Create<DataOptions>(new DataOptions()
            {
                JsonFilePath = "./../../../../data/users.json"
            }), configuration);

            var all = r.QueryAll(1);

            Assert.Equal(12, all.Entities.Count());
        }

        [Theory]
        [InlineData(10000)]
        public void Delete_ShouldRemoveTheUserFromTheList(int id)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var r = new UserRepository(Options.Create<DataOptions>(new DataOptions()
            {
                JsonFilePath = "./../../../../data/users.json"
            }), configuration);

            r.DeleteUser(id);

            var first = r.QueryAll(1).Entities.FirstOrDefault();

            Assert.NotNull(first);
            Assert.Equal(10001, first.Id);
        }
    }
}
