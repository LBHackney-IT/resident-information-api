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
    public class AcademyInformationGatewayTests
    {
        private Fixture _fixture;
        private AcademyInformationGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private string _currentEnv;

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
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, expectedJsonString: "[]");
            var received = await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();

        }

        [Test]
        public async Task GetClaimantInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { Address = "Address Line 1" };
            var expected = _fixture.CreateMany<AcademyClaimantInformation>();
            var expectedJson = JsonConvert.SerializeObject(expected);
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "?address=" + rqp.Address, expectedJson);

            var received = await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }
    }
}
