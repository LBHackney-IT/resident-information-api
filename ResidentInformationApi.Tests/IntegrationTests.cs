using System.Net.Http;
using NUnit.Framework;

namespace ResidentInformationApi.Tests
{
    public class IntegrationTests<TStartup> where TStartup : class
    {
        protected HttpClient Client { get; private set; }

        private MockWebApplicationFactory<TStartup> _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // any setup code required
        }

        [SetUp]
        public void BaseSetup()
        {
            _factory = new MockWebApplicationFactory<TStartup>();
            Client = _factory.CreateClient();
        }

        [TearDown]
        public void BaseTearDown()
        {
            Client.Dispose();
            _factory.Dispose();
        }
    }
}
