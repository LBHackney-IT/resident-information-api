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
using System.Collections.Generic;

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
        private HttpClient _httpClient;

        private string _apiToken;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("ACADEMY_API_URL");
            Environment.SetEnvironmentVariable("ACADEMY_API_URL", _uri.OriginalString);
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _apiToken = Environment.GetEnvironmentVariable("ACADEMY_API_TOKEN");
            Environment.SetEnvironmentVariable("ACADEMY_API_TOKEN", "secretKey");

            _httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };

            //_httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("ACADEMY_API_TOKEN"));

            _classUnderTest = new AcademyInformationGateway(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("ACADEMY_API_URL", _currentEnv);
            Environment.SetEnvironmentVariable("ACADEMY_API_TOKEN", _apiToken);
        }

        [Test]
        public async Task ApiKeySuccessfullyCalled()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "claimants", expectedJsonString: "{claimants: []}", expectedApiToken: "secretKey");
            await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);
            _messageHandler.Verify();

        }

        [Test]
        public async Task GetClaimantInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "claimants", expectedJsonString: "{claimants: []}");
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
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "claimants", "?address=" + rqp.Address, "{claimants:" + expectedJson + "}");

            var received = await _classUnderTest.GetClaimantInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetResidentInformationThrowsErrorIfAPIReturnsBadRequest()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnErrorCode(_messageHandler);
            Func<Task<List<AcademyClaimantInformation>>> testFunction = () => _classUnderTest.GetClaimantInformation(rqp);

            await testFunction.Should().ThrowAsync<HttpRequestException>().ConfigureAwait(true);
        }
    }
}
