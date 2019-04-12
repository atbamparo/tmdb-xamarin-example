using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Test.Repository.Handler
{
    [TestClass]
    public class HandlerTest
    {
        public Mock<HttpClientHandler> HttpHandler;
        public const string ApiKey = "API-KEY";

        public IRequestHandler Handler;

        [TestInitialize]
        public void Init()
        {
            HttpHandler = new Mock<HttpClientHandler>(MockBehavior.Strict);
            Handler = new RequestHandler3(new HttpClient(HttpHandler.Object), ApiKey);
        }

        [TestMethod]
        public async Task ShouldPrependBaseUrlToRquestsAndAppendApiKey()
        {
            SetupClientHandlerResponseMock(HttpStatusCode.OK, new StringContent(@"{ ""id"": ""1"" }"));

            var result = await Handler.SendAsync<MovieData>(HttpMethod.Get, "test");
            Assert.AreEqual("1", result.Id);
            VerifyHttpClientHandlerMock(HttpMethod.Get, $"https://api.themoviedb.org/3/test?api_key={ApiKey}");
        }

        [TestMethod]
        public async Task ShouldAppendApiKeyEvenIfQueryParametersArePassed()
        {
            SetupClientHandlerResponseMock(HttpStatusCode.OK, new StringContent(@"{ ""id"": ""1"" }"));

            var result = await Handler.SendAsync<MovieData>(HttpMethod.Get, "test?query=value");
            Assert.AreEqual("1", result.Id);
            VerifyHttpClientHandlerMock(HttpMethod.Get, $"https://api.themoviedb.org/3/test?query=value&api_key={ApiKey}");
        }

        [TestMethod]
        public async Task ShouldThrowIfRequestFail()
        {
            SetupClientHandlerResponseMock(HttpStatusCode.NotFound, new StringContent(@"{ ""id"": ""1"" }"));

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => Handler.SendAsync<MovieData>(HttpMethod.Get, "test?query=value"));
            VerifyHttpClientHandlerMock(HttpMethod.Get, $"https://api.themoviedb.org/3/test?query=value&api_key={ApiKey}");
        }

        [TestMethod]
        public async Task ShouldParseResponseInJsonSnakeCase()
        {
            SetupClientHandlerResponseMock(HttpStatusCode.OK, new StringContent(@"{ ""original_title"": ""Hoccus Poccus"" }"));

            var result = await Handler.SendAsync<MovieData>(HttpMethod.Get, "test?query=value");
            Assert.AreEqual("Hoccus Poccus", result.OriginalTitle);
            VerifyHttpClientHandlerMock(HttpMethod.Get, $"https://api.themoviedb.org/3/test?query=value&api_key={ApiKey}");
        }

        private void SetupClientHandlerResponseMock(HttpStatusCode statusCode, HttpContent content)
        {
            HttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(statusCode)
                {
                    Content = content
                })
                .Verifiable();
        }

        private void VerifyHttpClientHandlerMock(HttpMethod expectedMethod, string expectedRequestUri, int times = 1)
        {
            HttpHandler.Protected()
                .Verify("SendAsync",
                    Times.Exactly(times),
                    ItExpr.Is<HttpRequestMessage>(m =>
                        m.Method == expectedMethod
                        && m.RequestUri == new Uri(expectedRequestUri)),
                    ItExpr.IsAny<CancellationToken>());
        }
    }
}
