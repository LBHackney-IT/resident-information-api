using ResidentInformationApi.V1.Factories;
using ResidentInformationApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace ResidentInformationApi.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTest
    {
        [Test]
        public void CanBeCreatedFromDatabaseEntity()
        {
            var databaseEntity = new DatabaseEntity();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
        }
    }
}
