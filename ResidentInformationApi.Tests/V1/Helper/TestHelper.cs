using System;
using System.Linq;
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
        public static void SetUpMessageHandlerToReturnJson(Mock<HttpMessageHandler> messageHandler, string endpoint, string rqpString = null, string expectedJsonString = null, string expectedApiKey = null)
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
                        req => CheckUrls(endpoint, rqpString, req.RequestUri.ToString(), req, expectedApiKey)),

                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(stubbedResponse)
                .Verifiable();
        }

        public static void SetUpMessageHandlerToReturnErrorCode(Mock<HttpMessageHandler> messageHandler)
        {
            var stubbedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(stubbedResponse)
                .Verifiable();
        }

        private static bool CheckUrls(string endpoint, string query, string receivedRequest, HttpRequestMessage req, string expectedApiKey)
        {

            if (expectedApiKey != null)
            {
                var headers = req.Headers;
                var correctApiKey = headers.Contains("X-API-Key") && req.Headers.GetValues("X-API-Key").First() == expectedApiKey;
                if (correctApiKey == false)
                {
                    return false;
                }
            }

            return HttpUtility.UrlDecode(receivedRequest) == $"http://test-domain-name.com/api/v1/{endpoint}{query}";


        }
    }
}
