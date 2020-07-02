// using System.Collections.Generic;
// using FluentAssertions;
// using NUnit.Framework;
// using ResidentInformationApi.V1.Boundary.Responses;
// using ResidentInformationApi.V1.Domain;
// using ResidentInformationApi.V1.Factories;
// using Address = ResidentInformationApi.V1.Domain.Address;
// using AddressResponse = ResidentInformationApi.V1.Boundary.Responses.Address;
// using ResidentInformation = ResidentInformationApi.V1.Domain.ResidentInformation;
// using ResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.ResidentInformation;

// namespace MosaicResidentInformationApi.Tests.V1.Factories
// {
//     public class ResponseFactoryTests
//     {
//         [Test]
//         public void CanMapResidentInformationFromDomainToResponse()
//         {
//             var domain = new ResidentInformation
//             {
//                 Uprn = "uprn",
//                 AddressList = new List<Address>
//                 {
//                     new Address
//                     {
//                         AddressLine1 = "addess11",
//                         AddressLine2 = "address22",
//                         AddressLine3 = "address33",
//                         PostCode = "Postcode"
//                     }
//                 },
//                 FirstName = "Name",
//                 LastName = "Last",
//                 NhsNumber = "nhs",
//                 DateOfBirth = "DOB",
//                 PhoneNumberList = new List<PhoneNumber>
//                 {
//                     new PhoneNumber
//                     {
//                         Number = "number",
//                         Type = PhoneType.Fax
//                     }
//                 },
//             };

//             var expectedResponse = new ResidentInformationResponse
//             {
//                 System = "academy",
//                 SystemId = "123456",
//                 SystemUrl = new System.Uri("https://academy-api.hackney.gov.uk"),
//                 Uprn = "uprn",
//                 AddressList = new List<AddressResponse>
//                 {
//                     new AddressResponse()
//                     {
//                         AddressLine1 = "addess11",
//                         AddressLine2 = "address22",
//                         AddressLine3 = "address33",
//                         PostCode = "Postcode"
//                     }
//                 },
//                 FirstName = "Name",
//                 LastName = "Last",
//                 NhsNumber = "nhs",
//                 DateOfBirth = "DOB",
//                 PhoneNumber = new List<Phone>
//                 {
//                     new Phone
//                     {
//                         PhoneNumber = "number",
//                         PhoneType = PhoneType.Fax
//                     }
//                 },
//             };

//             domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
//         }

//         [Test]
//         [Ignore("to review")]
//         public void CanMapResidentInformationWithOnlyPersonalInformationFromDomainToResponse()
//         {
//             var domain = new ResidentInformation
//             {
//                 Uprn = "uprn",
//                 AddressList = null,
//                 FirstName = "Name",
//                 LastName = "Last",
//                 NhsNumber = "nhs",
//                 DateOfBirth = "DOB",
//                 PhoneNumberList = null,
//             };

//             var expectedResponse = new ResidentInformationResponse
//             {
//                 System = "academy",
//                 SystemId = "123456",
//                 SystemUrl = new System.Uri("https://academy-api.hackney.gov.uk"),
//                 Uprn = "uprn",
//                 AddressList = null,
//                 FirstName = "Name",
//                 LastName = "Last",
//                 NhsNumber = "nhs",
//                 DateOfBirth = "DOB",
//                 PhoneNumber = null,
//             };

//             domain.ToResponse().Should().BeEquivalentTo(expectedResponse);
//         }
//     }
// }
