using System;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using WireMock.Server;
using WireMock.Settings;

namespace ResidentInformationApi.Tests
{
    public class IntegrationTests<TStartup> where TStartup : class
    {
        protected HttpClient Client { get; private set; }
        protected WireMockServer MockAcademyAPI { get; private set; }
        protected WireMockServer MockHousingApi { get; private set; }
        protected WireMockServer MockMosaicApi { get; private set; }
        private MockWebApplicationFactory<TStartup> _factory;

        [SetUp]
        public void BaseSetup()
        {
            ConfigureMockApis();
            _factory = new MockWebApplicationFactory<TStartup>();
            Client = _factory.CreateClient();
        }

        [TearDown]
        public void BaseTearDown()
        {
            Client.Dispose();
            _factory.Dispose();
            MockAcademyAPI.Stop();
            MockAcademyAPI.Dispose();
            MockHousingApi.Stop();
            MockHousingApi.Dispose();
            MockMosaicApi.Stop();
            MockMosaicApi.Dispose();
        }

        private void ConfigureMockApis()
        {
            MockAcademyAPI = WireMockServer.Start();
            MockHousingApi = WireMockServer.Start();
            MockMosaicApi = WireMockServer.Start();
            Environment.SetEnvironmentVariable("ACADEMY_API_URL", $"http://localhost:{MockAcademyAPI.Ports[0]}/");
            Environment.SetEnvironmentVariable("HOUSING_API_URL", $"http://localhost:{MockHousingApi.Ports[0]}/");
            Environment.SetEnvironmentVariable("MOSAIC_API_URL", $"http://localhost:{MockMosaicApi.Ports[0]}/");
        }
    }
}
