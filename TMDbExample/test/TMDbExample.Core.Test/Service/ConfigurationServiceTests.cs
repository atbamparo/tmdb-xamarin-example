using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;
using TMDbExample.Core.Service;
using TMDbExample.Core.Service.API;

namespace TMDbExample.Core.Test.Service
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        public Mock<IConfigurationRepository> ConfigurationRepositoryMock = new Mock<IConfigurationRepository>(MockBehavior.Strict);
        public IConfigurationService ConfigurationService;

        [TestInitialize]
        public void Init()
        {
            ConfigurationService = new ConfigurationService(ConfigurationRepositoryMock.Object);
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetGenresById()
        {
            ConfigureMockWithBasicData();

            await ConfigurationService.ConfigureIfNeededAsync();
            var genreName = ConfigurationService.GetGenre("2");

            Assert.AreEqual("Romance", genreName);
        }

        [TestMethod]
        public async Task GenresShouldBeCachedByService()
        {
            ConfigureMockWithBasicData();

            await ConfigurationService.ConfigureIfNeededAsync();
            var genreName1 = ConfigurationService.GetGenre("1");
            var genreName2 = ConfigurationService.GetGenre("2");
            var genreName3 = ConfigurationService.GetGenre("3");

            Assert.AreEqual("Action", genreName1);
            Assert.AreEqual("Romance", genreName2);
            Assert.AreEqual("Comedy", genreName3);

            ConfigurationRepositoryMock.Verify(r => r.GetGenresAsync(), Times.Once);
        }

        [TestMethod]
        public async Task InternalServiceConfigurationShouldNotBeChanged()
        {
            ConfigureMockWithBasicData();

            await ConfigurationService.ConfigureIfNeededAsync();
            var config = ConfigurationService.GetImageConfiguration();
            config.BaseUrl = "https://changed.base.url";
            config.BackdropSizes.Add("changedSize");
            config.PosterSizes.Add("changedSize");

            var newConfig = ConfigurationService.GetImageConfiguration();
            Assert.AreEqual("https://base.url", newConfig.BaseUrl);
            CollectionAssert.AreEqual(new[] { "w100", "w200" }, newConfig.BackdropSizes);
            CollectionAssert.AreEqual(new[] { "w100", "w200" }, newConfig.BackdropSizes);

            ConfigurationRepositoryMock.Verify(r => r.GetConfigurationAsync(), Times.Once);
        }

        [TestMethod]
        public async Task MultipleCallsToConfigureShouldFetchDataOnlyOnce()
        {
            ConfigureMockWithBasicData();

            await ConfigurationService.ConfigureIfNeededAsync();
            await ConfigurationService.ConfigureIfNeededAsync();
            await ConfigurationService.ConfigureIfNeededAsync();

            ConfigurationRepositoryMock.Verify(r => r.GetConfigurationAsync(), Times.Once);
        }

        [TestMethod]
        public async Task MultipleCallsWhileConfiguringShouldNotFetchDataMoreThanOnce()
        {
            var releaseConfigurationTask = new TaskCompletionSource<ConfigurationData>();
            ConfigurationRepositoryMock.Setup(r => r.GetConfigurationAsync())
               .Returns(releaseConfigurationTask.Task)
               .Verifiable();

            ConfigurationRepositoryMock.Setup(r => r.GetGenresAsync())
                .ReturnsAsync(CreateBasicGenresData())
                .Verifiable();

            var call1 = ConfigurationService.ConfigureIfNeededAsync();
            var call2 = ConfigurationService.ConfigureIfNeededAsync();
            var call3 = ConfigurationService.ConfigureIfNeededAsync();
            releaseConfigurationTask.SetResult(CreateBasicConfigurationData());
            await Task.WhenAll(call1, call2, call3);

            ConfigurationRepositoryMock.Verify(r => r.GetConfigurationAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ConfigureAfterFailedConfigureAttemptShouldTryFetchDataAgain()
        {
            ConfigurationRepositoryMock.SetupSequence(r => r.GetConfigurationAsync())
               .ThrowsAsync(new Exception())
               .ReturnsAsync(CreateBasicConfigurationData());

            ConfigurationRepositoryMock.Setup(r => r.GetGenresAsync())
                .ReturnsAsync(CreateBasicGenresData())
                .Verifiable();

            await Assert.ThrowsExceptionAsync<Exception>(() => ConfigurationService.ConfigureIfNeededAsync());
            await ConfigurationService.ConfigureIfNeededAsync();

            ConfigurationRepositoryMock.Verify(r => r.GetConfigurationAsync(), Times.Exactly(2));
        }


        private void ConfigureMockWithBasicData()
        {
            ConfigurationRepositoryMock.Setup(r => r.GetConfigurationAsync())
               .ReturnsAsync(CreateBasicConfigurationData())
               .Verifiable();

            ConfigurationRepositoryMock.Setup(r => r.GetGenresAsync())
                .ReturnsAsync(CreateBasicGenresData())
                .Verifiable();
        }

        private IEnumerable<GenreData> CreateBasicGenresData() =>
            new[]
            {
                new GenreData { Id = "1", Name = "Action" },
                new GenreData { Id = "2", Name = "Romance" },
                new GenreData { Id = "3", Name = "Comedy" }
            };

        private ConfigurationData CreateBasicConfigurationData() =>
            new ConfigurationData
            {
                Images = new ImagesData
                {
                    BaseUrl = "https://base.url",
                    BackdropSizes = new List<string> { "w100", "w200" },
                    PosterSizes = new List<string> { "w100", "w200" }
                }
            };

    }
}
