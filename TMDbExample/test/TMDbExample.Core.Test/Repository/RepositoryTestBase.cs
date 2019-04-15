using Moq;
using System.Net.Http;
using TMDbExample.Core.Repository;

namespace TMDbExample.Core.Test.Repository
{
    public abstract class RepositoryTestBase
    {
        protected Mock<IRequestHandler> HandlerMock = new Mock<IRequestHandler>(MockBehavior.Strict);

        protected void SetupHandlerResult<TResult>(TResult result)
        {
            HandlerMock.Setup(h => h.SendAsync<TResult>(It.IsAny<HttpMethod>(), It.IsAny<string>()))
                .ReturnsAsync(result)
                .Verifiable();
        }

        protected void VerifyHandlerCall<TResult>(HttpMethod expectedMethod, string expectedRequestUri, int times = 1)
        {
            HandlerMock.Verify(
                    h => h.SendAsync<TResult>(expectedMethod, expectedRequestUri),
                    Times.Exactly(times));
        }
    }
}
