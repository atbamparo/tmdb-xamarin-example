using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Test.Repository.Movies
{
    [TestClass]
    public class SingleMovieTests : RepositoryTestBase
    {
        public IMoviesRepository Repository;

        [TestInitialize]
        public void Init()
        {
            Repository = new MoviesRepository(HandlerMock.Object);
        }

        [TestMethod]
        public async Task GetExistingMovieShouldWork()
        {
            SetupHandlerResult(new MovieData
            {
                Id = "76341",
                Title = "Mad Max: Fury Road",
                OriginalTitle = "Mad Max: Fury Road"
            });

            var movie = await Repository.GetMovieAsync("76341");

            Assert.AreEqual("76341", movie.Id);
            Assert.AreEqual("Mad Max: Fury Road", movie.OriginalTitle);
            Assert.AreEqual("Mad Max: Fury Road", movie.Title);
            VerifyHandlerCall<MovieData>(HttpMethod.Get, "movie/76341?language=en-US");
        }

        [TestMethod]
        public async Task GetMovieInAnotherLanguageShouldWork()
        {
            SetupHandlerResult(new MovieData
            {
                Id = "76341",
                Title = "Mad Max: Estrada da F�ria",
                OriginalTitle = "Mad Max: Fury Road"
            });

            var movie = await Repository.GetMovieAsync("76341", "pt-BR");

            Assert.AreEqual("76341", movie.Id);
            Assert.AreEqual("Mad Max: Fury Road", movie.OriginalTitle);
            Assert.AreEqual("Mad Max: Estrada da F�ria", movie.Title);
            VerifyHandlerCall<MovieData>(HttpMethod.Get, "movie/76341?language=pt-BR");
        }


        [TestMethod]
        public async Task GetInvalidMovieIdShouldThrowException()
        {
            HandlerMock.Setup(h => h.SendAsync<MovieData>(It.IsAny<HttpMethod>(), It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException());

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => Repository.GetMovieAsync("76341"));
            VerifyHandlerCall<MovieData>(HttpMethod.Get, "movie/76341?language=en-US");
        }
    }
}
