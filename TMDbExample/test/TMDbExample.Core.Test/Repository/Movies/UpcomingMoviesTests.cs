using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Test.Repository.Movies
{
    [TestClass]
    public class UpcomingMoviesTests : RepositoryTestBase
    {
        public IMoviesRepository Repository;

        [TestInitialize]
        public void Init()
        {
            Repository = new MoviesRepository(HandlerMock.Object);
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetCurrentlyPlayingMovies()
        {
            SetupHandlerResult(CreateBasicUpcomingMoviesData());

            var currentlyPlaying = await Repository.GetUpcomingMoviesAsync();

            Assert.AreEqual(1, currentlyPlaying.Page);
            Assert.AreEqual(1, currentlyPlaying.TotalPages);
            Assert.AreEqual(2, currentlyPlaying.TotalResults);
            Assert.AreEqual(2, currentlyPlaying.Results.Count());
            var results = currentlyPlaying.Results.ToList();
            Assert.AreEqual("1", results[0].Id);
            Assert.AreEqual("2", results[1].Id);
            VerifyHandlerCall<UpcomingMoviesData>(HttpMethod.Get, "movie/upcoming?page=1&language=en-US&region=US");
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetDifferentPagesOfCurrentlyPlayingMovies()
        {
            SetupHandlerResult(CreateBasicUpcomingMoviesData());

            var currentlyPlaying = await Repository.GetUpcomingMoviesAsync(2);

            Assert.AreEqual(1, currentlyPlaying.Page);
            Assert.AreEqual(1, currentlyPlaying.TotalPages);
            Assert.AreEqual(2, currentlyPlaying.TotalResults);
            Assert.AreEqual(2, currentlyPlaying.Results.Count());
            var results = currentlyPlaying.Results.ToList();
            Assert.AreEqual("1", results[0].Id);
            Assert.AreEqual("2", results[1].Id);
            VerifyHandlerCall<UpcomingMoviesData> (HttpMethod.Get, "movie/upcoming?page=2&language=en-US&region=US");
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetCurrentlyPlayingMoviesInAnotherLanguage()
        {
            SetupHandlerResult(CreateBasicUpcomingMoviesData());

            var currentlyPlaying = await Repository.GetUpcomingMoviesAsync(language: "pt-BR");

            Assert.AreEqual(1, currentlyPlaying.Page);
            Assert.AreEqual(1, currentlyPlaying.TotalPages);
            Assert.AreEqual(2, currentlyPlaying.TotalResults);
            Assert.AreEqual(2, currentlyPlaying.Results.Count());
            var results = currentlyPlaying.Results.ToList();
            Assert.AreEqual("1", results[0].Id);
            Assert.AreEqual("2", results[1].Id);
            VerifyHandlerCall<UpcomingMoviesData>(HttpMethod.Get, "movie/upcoming?page=1&language=pt-BR&region=US");
        }

        [TestMethod]
        public async Task ShouldBePossibleToGetCurrentlyPlayingMoviesInAnotherRegion()
        {
            SetupHandlerResult(CreateBasicUpcomingMoviesData());

            var currentlyPlaying = await Repository.GetUpcomingMoviesAsync(region: "BR");

            Assert.AreEqual(1, currentlyPlaying.Page);
            Assert.AreEqual(1, currentlyPlaying.TotalPages);
            Assert.AreEqual(2, currentlyPlaying.TotalResults);
            Assert.AreEqual(2, currentlyPlaying.Results.Count());
            var results = currentlyPlaying.Results.ToList();
            Assert.AreEqual("1", results[0].Id);
            Assert.AreEqual("2", results[1].Id);
            VerifyHandlerCall<UpcomingMoviesData>(HttpMethod.Get, "movie/upcoming?page=1&language=en-US&region=BR");
        }

        private UpcomingMoviesData CreateBasicUpcomingMoviesData() =>
            new UpcomingMoviesData
            {
                Page = 1,
                TotalPages = 1,
                TotalResults = 2,
                Results = new List<MovieListData> {
                    new MovieListData
                    {
                        Id = "1"
                    },
                    new MovieListData
                    {
                        Id = "2"
                    }
                }
            };
    }
}
