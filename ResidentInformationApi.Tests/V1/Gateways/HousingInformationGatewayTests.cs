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
    public class HousingInformationGatewayTests
    {
        private Fixture _fixture;
        private HousingInformationGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private string _currentEnv;
        private string _apiToken;


        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("HOUSING_API_URL");
            Environment.SetEnvironmentVariable("HOUSING_API_URL", _uri.OriginalString);
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _apiToken = Environment.GetEnvironmentVariable("HOUSING_API_TOKEN");
            Environment.SetEnvironmentVariable("HOUSING_API_TOKEN", "secretKey");

            var _httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("HOUSING_API_TOKEN"));
            _classUnderTest = new HousingInformationGateway(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("HOUSING_API_URL", _currentEnv);
            Environment.SetEnvironmentVariable("HOUSING_API_TOKEN", _apiToken);

        }

        [Test]
        public async Task ApiKeySuccessfullyCalled()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "households", expectedJsonString: "{residents: []}", expectedApiToken: "secretKey");
            await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);
            _messageHandler.Verify();

        }

        [Test]
        public async Task GetResidentInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "households", expectedJsonString: "{residents: []}");
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
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "households", "?address=" + rqp.Address, "{residents: " + expectedJson + "}");

            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetResidentInformationThrowsErrorIfAPIReturnsBadRequest()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnErrorCode(_messageHandler);
            Func<Task<List<HousingResidentInformation>>> testFunction = () => _classUnderTest.GetResidentInformation(rqp);

            await testFunction.Should().ThrowAsync<HttpRequestException>().ConfigureAwait(true);
        }
    }
}
