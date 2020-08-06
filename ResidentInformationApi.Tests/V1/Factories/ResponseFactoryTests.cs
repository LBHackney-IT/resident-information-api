using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Factories;
using AcademyClaimantInformation = ResidentInformationApi.V1.Domain.AcademyClaimantInformation;
using AcademyClaimantInformationResponse = ResidentInformationApi.V1.Boundary.Responses.AcademyClaimantInformation;
using Address = ResidentInformationApi.V1.Domain.Address;
using AddressResponse = ResidentInformationApi.V1.Boundary.Responses.Address;
using HousingResidentInformation = ResidentInformationApi.V1.Domain.HousingResidentInformation;
using HousingResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.HousingResidentInformation;
using MosaicResidentInformation = ResidentInformationApi.V1.Domain.MosaicResidentInformation;
using MosaicResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.MosaicResidentInformation;
using PhoneNumberResponse = ResidentInformationApi.V1.Boundary.Responses.Phone;
using PhoneType = ResidentInformationApi.V1.Domain.PhoneType;
using PhoneTypeResponse = ResidentInformationApi.V1.Boundary.Responses.PhoneType;

namespace ResidentInformationApi.Tests.V1.Factories
{
    public class ResponseFactoryTests
    {
        [Test]
        public void CanMapAcademyInformationFromDomainToResponse()
        {
            var domain = new AcademyClaimantInformation
            {
                ClaimId = 123,
                PersonRef = 456,
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = "01/01/2001",
                NINumber = "NI123456N",
                ClaimantAddress = new Address
                {
                    AddressLine1 = "addess11",
                    AddressLine2 = "address22",
                    AddressLine3 = "address33",
                    PostCode = "Postcode"
                },
                CheckDigit = "Digit"
            };

            var expectedResponse = new AcademyClaimantInformationResponse
            {
                ClaimId = 123,
                PersonRef = 456,
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = "01/01/2001",
                NINumber = "NI123456N",
                ClaimantAddress = new AddressResponse
                {
                    AddressLine1 = "addess11",
                    AddressLine2 = "address22",
                    AddressLine3 = "address33",
                    PostCode = "Postcode"
                },
                CheckDigit = "Digit"
            };

            domain.ToResponse().Should().BeEquivalentTo(expectedResponse);

        }
        [Test]
        public void CanMapHousingInformationFromDomainToResponse()
        {
            var domain = new HousingResidentInformation
            {
                Uprn = "uprn",
                Address = new HousingAddress()
                {
                    AddressLine1 = "addess11",
                    PropertyRef = "my prop ref",
                    PostCode = "Postcode"
                },
                FirstName = "Name",
                LastName = "Last",
                DateOfBirth = "DOB",
                PhoneNumber = new List<HousingPhone>
                {
                    new HousingPhone()
                    {
                        PhoneNumber = "number",
                        PhoneType = HousingPhoneTypeEnum.F
                    }
                },
            };

            var expectedResponse = new HousingResidentInformationResponse
            {
                Uprn = "uprn",
                Address = new AddressResponse()
                {
                    AddressLine1 = "addess11",
                    PostCode = "Postcode"
                },
                FirstName = "Name",
                LastName = "Last",
                DateOfBirth = "DOB",
                PhoneNumber = new List<PhoneNumberResponse>
                {
                    new PhoneNumberResponse
                    {
                        PhoneNumber = "number",
                        PhoneType = PhoneTypeResponse.Fax
                    }
                }
            };

            domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapHousingInformationWithOnlyPersonalInformationFromDomainToResponse()
        {
            var domain = new HousingResidentInformation
            {
                Uprn = "uprn",
                Address = null,
                FirstName = "Name",
                LastName = "Last",
                DateOfBirth = "DOB",
                PhoneNumber = null,
            };

            var expectedResponse = new HousingResidentInformationResponse
            {
                Uprn = "uprn",
                Address = null,
                FirstName = "Name",
                LastName = "Last",
                DateOfBirth = "DOB",
                PhoneNumber = null
            };

            domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapMosaicInformationFromDomainToResponse()
        {
            var domain = new MosaicResidentInformation
            {
                MosaicId = "123",
                FirstName = "First",
                LastName = "Last",
                Uprn = "100000000",
                DateOfBirth = "01/01/2001",
                PhoneNumber = new List<Phone>
                {
                    new Phone()
                    {
                        PhoneNumber = "07894564561",
                        PhoneType = PhoneType.Home.ToString()
                    }
                },
                AddressList = new List<Address>
                {
                    new Address
                    {
                        AddressLine1 = "addess11",
                        AddressLine2 = "address22",
                        AddressLine3 = "address33",
                        PostCode = "Postcode"
                    }
                },
                NhsNumber = "2000000000"
            };

            var expectedResponse = new MosaicResidentInformationResponse
            {
                MosaicId = "123",
                FirstName = "First",
                LastName = "Last",
                Uprn = "100000000",
                DateOfBirth = "01/01/2001",
                PhoneNumber = new List<PhoneNumberResponse>
                {
                    new PhoneNumberResponse
                    {
                        PhoneNumber = "07894564561",
                        PhoneType = PhoneTypeResponse.Home
                    }
                },
                AddressList = new List<AddressResponse>
                {
                    new AddressResponse
                    {
                        AddressLine1 = "addess11",
                        AddressLine2 = "address22",
                        AddressLine3 = "address33",
                        PostCode = "Postcode"
                    }
                },
                NhsNumber = "2000000000"
            };

            domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
        }
        [Test]
        public void CanMapMosaicInformationWithOnlyPersonalInformationFromDomainToResponse()
        {
            var domain = new MosaicResidentInformation
            {
                MosaicId = "123",
                FirstName = "First",
                LastName = "Last",
                Uprn = "100000000",
                DateOfBirth = "01/01/2001",
                AddressList = null,
                PhoneNumber = null,
                NhsNumber = "2000000000"

            };

            var expectedResponse = new MosaicResidentInformationResponse
            {
                MosaicId = "123",
                FirstName = "First",
                LastName = "Last",
                Uprn = "100000000",
                DateOfBirth = "01/01/2001",
                AddressList = null,
                PhoneNumber = null,
                NhsNumber = "2000000000"
            };

            domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
        }
    }
}
