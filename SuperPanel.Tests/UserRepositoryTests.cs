using Microsoft.Extensions.Options;
using SuperPanel.Config;
using SuperPanel.DataProvider;
using System.Linq;
using Xunit;

namespace SuperPanel.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void QueryAll_ShouldReturnEverything()
        {
            var r = new UserRepository(Options.Create<DataOptions>(new DataOptions()
            {
                JsonFilePath = "./../../../../data/users.json"
            }));

            var all = r.QueryAll();

            Assert.Equal(5000, all.Count());
        }
    }
}
