using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Test.Repository.Configuration
{
    [TestClass]
    public class ConfigurationTests : RepositoryTestBase
    {
        public IConfigurationRepository Repository;

        [TestInitialize]
        public void Init()
        {
            Repository = new ConfigurationRepository(HandlerMock.Object);
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetConfiguration()
        {
            SetupHandlerResult(new ConfigurationData
            {
                Images = new ImagesData
                {
                    BaseUrl = "https://tests.com"
                }
            });

            var result = await Repository.GetConfigurationAsync();

            Assert.AreEqual("https://tests.com", result.Images.BaseUrl);
            VerifyHandlerCall<ConfigurationData>(HttpMethod.Get, "configuration");
        }
    }
}
