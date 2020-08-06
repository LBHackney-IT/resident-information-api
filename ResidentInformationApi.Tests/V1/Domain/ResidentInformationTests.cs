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
            var phoneNumber = new Phone()
            {
                PhoneNumber = "1234567890",
                PhoneType = PhoneType.Home.ToString()
            };

            var address = new Address
            {
                AddressLine1 = "Address Line 1",
                AddressLine2 = "Address Line 2",
                AddressLine3 = "Address Line 3",
                PostCode = "AB1 2BC",
            };

            var residentInformation = new HousingResidentInformation
            {

                FirstName = "First",
                LastName = "Last",
                Uprn = "abc123",
                DateOfBirth = "1980-10-02",
                PhoneNumber = new List<Phone> { phoneNumber },
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
