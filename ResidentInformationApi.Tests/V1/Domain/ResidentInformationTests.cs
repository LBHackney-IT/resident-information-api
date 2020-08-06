using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.Tests.V1.Domain
{
    [TestFixture]
    public class ResidentInformationTests
    {
        [Test]
        public void ResidentInformationIncludesRequiredProperties()
        {
            var phoneNumber = new HousingPhone()
            {
                PhoneNumber = "1234567890",
                PhoneType = HousingPhoneTypeEnum.H
            };

            var address = new HousingAddress
            {
                AddressLine1 = "Address Line 1",
                PropertyRef = "my property ref",
                PostCode = "AB1 2BC",
            };

            var residentInformation = new HousingResidentInformation
            {

                FirstName = "First",
                LastName = "Last",
                Uprn = "abc123",
                DateOfBirth = "1980-10-02",
                PhoneNumber = new List<HousingPhone> { phoneNumber },
                Address = address,
            };

            residentInformation.FirstName.Should().Be("First");
            residentInformation.LastName.Should().Be("Last");
            residentInformation.Uprn.Should().Be("abc123");
            residentInformation.DateOfBirth.Should().Be("1980-10-02");
            residentInformation.PhoneNumber.Should().Contain(phoneNumber);
            residentInformation.Address.Should().BeEquivalentTo(address);
        }
    }
}
