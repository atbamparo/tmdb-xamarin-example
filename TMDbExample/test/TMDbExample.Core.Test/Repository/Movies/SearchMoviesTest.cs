using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Test.Repository.Movies
{
    [TestClass]
    public class SearchMoviesTest : RepositoryTestBase
    {
        public IMoviesRepository Repository;

        [TestInitialize]
        public void Init()
        {
            Repository = new MoviesRepository(HandlerMock.Object);
        }

        [TestMethod]
        public async Task QueryStringShouldBeUrlEncoded()
        {
            SetupHandlerResult(new MoviesData {
                TotalResults = 0,
                TotalPages = 0,
                Page = 1,
                Results = Enumerable.Empty<MovieListData>().ToList()
            });

            await Repository.SearchMoviesAsync("-Romeo & +Juliet");
            VerifyHandlerCall<MoviesData>(HttpMethod.Get, "search/movie?query=-Romeo+%26+%2bJuliet&page=1");
        }

        [TestMethod]
        public async Task EmptyQueryShouldThrowWithoutTryToFetchingData()
        {
            SetupHandlerResult<MoviesData>(null);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Repository.SearchMoviesAsync(string.Empty));
            HandlerMock.Verify(h => h.SendAsync<MoviesData>(It.IsAny<HttpMethod>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task NullQueryShouldThrowWithoutTryToFetchingData()
        {
            SetupHandlerResult<MoviesData>(null);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Repository.SearchMoviesAsync(null));
            HandlerMock.Verify(h => h.SendAsync<MoviesData>(It.IsAny<HttpMethod>(), It.IsAny<string>()), Times.Never);
        }
    }
}
