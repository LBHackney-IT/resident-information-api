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
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways;

namespace ResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class HousingInformationGatewayTests
    {
        private Fixture _fixture;
        private HousingInformationGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private String _currentEnv;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("HOUSING_API_ENDPOINT");
            Environment.SetEnvironmentVariable("HOUSING_API_ENDPOINT", _uri.OriginalString);
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };

            _classUnderTest = new HousingInformationGateway(httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("HOUSING_API_ENDPOINT", _currentEnv);
        }

        [Test]
        public async Task GetResidentInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            SetUpMessageHandlerToReturnJson(expectedJsonString: "[]");
            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();
        }

        [Test]
        public async Task GetResidentInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { Address = "Address Line 1" };
            var expected = new List<HousingResidentInformation> { _fixture.Create<HousingResidentInformation>() };
            var expectedJson = JsonConvert.SerializeObject(expected);
            SetUpMessageHandlerToReturnJson("?address=" + rqp.Address, expectedJson);

            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

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
                            ) == "http://test-domain-name.com/" + rqpString
                        ),
                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(stubbedResponse)
                .Verifiable();
        }
    }
}
