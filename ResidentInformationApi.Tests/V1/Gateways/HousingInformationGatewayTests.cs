using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ResidentInformationApi.Tests.V1.Helper;
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
        private string _currentEnv;

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
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "households", expectedJsonString: "[]");
            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();
        }

        [Test]
        public async Task GetResidentInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { Address = "Address Line 1" };
            var expected = _fixture.CreateMany<HousingResidentInformation>();
            var expectedJson = JsonConvert.SerializeObject(expected);
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "households", "?address=" + rqp.Address, expectedJson);

            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }
    }
}
