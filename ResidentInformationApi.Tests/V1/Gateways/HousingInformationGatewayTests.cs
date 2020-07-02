using AutoFixture;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways;
using FluentAssertions;
using NUnit.Framework;
// using ResidentInformationApi.Tests.V1.Helper;

namespace ResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class HousingInformationGatewayTests
    {
        private Fixture _fixture = new Fixture();
        private HousingInformationGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new HousingInformationGateway();
        }

        [Test]
        public void GetResidentInformationByAddressReturnsEmptyArrayIfNoResultsFound()
        {
            var response = _classUnderTest.GetResidentInformationByAddress("");

            response.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetResidentInformationByAddressReturnsArrayOfResidentInformationObjects()
        {

            var expected = _fixture.Create<ResidentInformation>();

            var received = _classUnderTest.GetResidentInformationByAddress("Address Line 1");

            received.Should().BeEquivalentTo(expected);


            // var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            // DatabaseContext.DatabaseEntities.Add(databaseEntity);
            // DatabaseContext.SaveChanges();

            // var response = _classUnderTest.GetEntityById(databaseEntity.Id);

            // databaseEntity.Id.Should().Be(response.Id);
        }
    }
}
