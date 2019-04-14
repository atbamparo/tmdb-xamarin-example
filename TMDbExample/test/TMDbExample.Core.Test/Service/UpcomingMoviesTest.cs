using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbExample.Core.Model;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;
using TMDbExample.Core.Service;
using TMDbExample.Core.Service.API;

namespace TMDbExample.Core.Test.Service
{
    [TestClass]
    public class UpcomingMoviesTest
    {
        public Mock<IMoviesRepository> MoviesRepositoryMock = new Mock<IMoviesRepository>(MockBehavior.Strict);
        public Mock<IConfigurationService> ConfigurationServiceMock = new Mock<IConfigurationService>(MockBehavior.Strict);

        public IMoviesService Service;

        [TestInitialize]
        public void Init()
        {
            Service = new MoviesService(ConfigurationServiceMock.Object, MoviesRepositoryMock.Object);
        }

        [TestMethod]
        public async Task ConfigurationServiceShouldBeInitializedOnGettingMovies()
        {
            ConfigureBasicMocks();

            await Service.GetUpcomingMoviesPageAsync(1);

            ConfigurationServiceMock.Verify(s => s.ConfigureIfNeededAsync(), Times.Once);
        }

        [TestMethod]
        public async Task MoviesShouldGetImageBasePathAndProperPosterAndBackdropImageSizesIfPossible()
        {
            ConfigureBasicMocks();

            var result = await Service.GetUpcomingMoviesPageAsync(1);

            Assert.AreEqual("https://images.base.path/pw4/poster_path/0.png", result.Results.First().PosterUrl);
            Assert.AreEqual("https://images.base.path/bw2/backdrop_path/0.png", result.Results.First().BackdropUrl);
            Assert.AreEqual("https://images.base.path/pw4/poster_path/1.png", result.Results.Last().PosterUrl);
            Assert.AreEqual("https://images.base.path/bw2/backdrop_path/1.png", result.Results.Last().BackdropUrl);
            ConfigurationServiceMock.Verify(s => s.GetImageConfiguration(), Times.Once);
        }

        [TestMethod]
        public async Task MoviesShouldGetImageBasePathAndMostClosePosterAndBackdropImageSizesIfProperIsUnavailable()
        {
            ConfigureBasicMocks();
            ConfigurationServiceMock.Setup(s => s.GetImageConfiguration())
                .Returns(new ImageConfiguration
                {
                    PosterSizes = new List<string> { "/pw1", "/pw2", "/pw3" },
                    BackdropSizes = new List<string> { "/bw1" },
                    BaseUrl = "https://images.base.path"
                });

            var result = await Service.GetUpcomingMoviesPageAsync(1);

            Assert.AreEqual("https://images.base.path/pw3/poster_path/0.png", result.Results.First().PosterUrl);
            Assert.AreEqual("https://images.base.path/bw1/backdrop_path/0.png", result.Results.First().BackdropUrl);
            Assert.AreEqual("https://images.base.path/pw3/poster_path/1.png", result.Results.Last().PosterUrl);
            Assert.AreEqual("https://images.base.path/bw1/backdrop_path/1.png", result.Results.Last().BackdropUrl);
            ConfigurationServiceMock.Verify(s => s.GetImageConfiguration(), Times.Once);
        }

        [TestMethod]
        public async Task GenresFromMoviesShouldHaveNameResolvedFromId()
        {
            ConfigureBasicMocks();
            var result = await Service.GetUpcomingMoviesPageAsync(1);

            CollectionAssert.AreEqual(new List<string> { "Genre #2", "Genre #3" }, result.Results.First().Genres.ToList());
            CollectionAssert.AreEqual(new List<string> { "Genre #3", "Genre #4" }, result.Results.Last().Genres.ToList());
        }

        private void ConfigureBasicMocks()
        {
            ConfigurationServiceMock.Setup(s => s.ConfigureIfNeededAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            ConfigurationServiceMock.Setup(s => s.GetImageConfiguration())
                .Returns(new ImageConfiguration
                {
                    PosterSizes = new List<string> { "/pw1", "/pw2", "/pw3", "/pw4", "/pw5" },
                    BackdropSizes = new List<string> { "/bw1", "/bw2", "/bw3", "/bw4", "/bw5" },
                    BaseUrl = "https://images.base.path"
                })
                .Verifiable();
            ConfigurationServiceMock.Setup(s => s.GetGenre(It.IsAny<string>()))
                .Returns<string>(genreId => $"Genre #{genreId}");

            MoviesRepositoryMock.Setup(s => s.GetUpcomingMoviesAsync(1, null, null))
                .ReturnsAsync(new UpcomingMoviesData
                {
                    Page = 1,
                    TotalPages = 1,
                    TotalResults = 2,
                    Results = CreateBasicMovieListData(2)
                })
                .Verifiable();
        }

        private List<MovieListData> CreateBasicMovieListData(int quantity) =>
            Enumerable.Range(0, quantity).Select(idx => new MovieListData
            {
                Title = $"Movie title #{idx}",
                Overview = $"Movie overview #{idx}",
                BackdropPath = $"/backdrop_path/{idx}.png",
                PosterPath = $"/poster_path/{idx}.png",
                ReleaseDate = new DateTime(2019, 04, 14).AddDays(idx).Date,
                GenreIds = new List<string> { $"{idx + quantity}", $"{idx + quantity + 1}" }
            }).ToList();
    }
}
