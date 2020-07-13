using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture;
using Moq;
using Moq.Protected;

namespace ResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static void SetUpMessageHandlerToReturnJson(Mock<HttpMessageHandler> messageHandler, string rqpString = null, string expectedJsonString = null)
        {
            if (expectedJsonString == null)
            {
                expectedJsonString = new Fixture().Create<string>();
            }
            var stubbedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJsonString)
            };

            messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        req => HttpUtility.UrlDecode(
                            req.RequestUri.ToString()
                            ) == "http://test-domain-name.com/" + rqpString
                        ),
                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(stubbedResponse)
                .Verifiable();
        }
    }
}
