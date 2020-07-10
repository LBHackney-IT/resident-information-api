using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Gateways;

namespace ResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class AcademyInformationGatewayTests
    {
        private Fixture _fixture;
        private AcademyInformationGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private String _currentEnv;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("ACADEMY_API_ENDPOINT");
            Environment.SetEnvironmentVariable("ACADEMY_API_ENDPOINT", _uri.OriginalString);
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };

            _classUnderTest = new AcademyInformationGateway(httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("ACADEMY_API_ENDPOINT", _currentEnv);
        }

        [Test]
        public async Task GetClaimantInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            SetUpMessageHandlerToReturnJson(expectedJsonString: "[]");
            var received = await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();

        }

        [Test]
        public async Task GetClaimantInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { Address = "Address Line 1" };
            var expected = new List<AcademyClaimantInformation> { _fixture.Create<AcademyClaimantInformation>() };
            var expectedJson = JsonConvert.SerializeObject(expected);
            SetUpMessageHandlerToReturnJson("?address=" + rqp.Address, expectedJson);

            var received = await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }

        private void SetUpMessageHandlerToReturnJson(string rqpString = null, string expectedJsonString = null)
        {
            if (expectedJsonString == null)
            {
                expectedJsonString = _fixture.Create<String>();
            }
            var stubbedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJsonString)
            };

            _messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        req => HttpUtility.UrlDecode(
                            req.RequestUri.ToString()
                            ) == "http://test-domain-name.com/" + rqpString),
                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(stubbedResponse)
                .Verifiable();
        }
    }
}
