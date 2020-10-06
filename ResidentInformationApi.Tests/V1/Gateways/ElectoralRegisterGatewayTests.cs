using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ResidentInformationApi.Tests.V1.Helper;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResidentInformationApi.Tests.V1.Gateways
{
    public class ElectoralRegisterGatewayTests
    {
        private Fixture _fixture;
        private ElectoralRegisterGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private string _currentEnv;
        private string _apiToken;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("ELECTORAL_REGISTER_API_URL");
            Environment.SetEnvironmentVariable("ELECTORAL_REGISTER_API_URL", _uri.OriginalString);
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _apiToken = Environment.GetEnvironmentVariable("ELECTORAL_REGISTER_API_TOKEN");
            Environment.SetEnvironmentVariable("ELECTORAL_REGISTER_API_TOKEN", "secretKey");

            var _httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("ELECTORAL_REGISTER_API_TOKEN"));
            _classUnderTest = new ElectoralRegisterGateway(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("HOUSING_API_URL", _currentEnv);
            Environment.SetEnvironmentVariable("HOUSING_API_TOKEN", _apiToken);
        }
        [Test]
        public async Task GetResidentInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "residents", expectedJsonString: "{residents: []}");
            var received = await _classUnderTest.GetResidentsInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();
        }

        [Test]
        public async Task GetResidentInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { FirstName = "John" };
            var expected = _fixture.CreateMany<ElectoralRegisterResidentInformation>();
            var expectedJson = JsonConvert.SerializeObject(expected);
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "residents", "?first_name=" + rqp.FirstName, "{residents: " + expectedJson + "}");

            var received = await _classUnderTest.GetResidentsInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetResidentInformationThrowsErrorIfAPIReturnsBadRequest()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnErrorCode(_messageHandler);
            Func<Task<List<ElectoralRegisterResidentInformation>>> testFunction = () => _classUnderTest.GetResidentsInformation(rqp);

            await testFunction.Should().ThrowAsync<HttpRequestException>().ConfigureAwait(true);
        }
    }
}
