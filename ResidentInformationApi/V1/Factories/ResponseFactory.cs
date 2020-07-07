using System.Collections.Generic;
using System.Linq;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using Address = ResidentInformationApi.V1.Domain.Address;
using AddressResponse = ResidentInformationApi.V1.Boundary.Responses.Address;
using HousingResidentInformation = ResidentInformationApi.V1.Domain.HousingResidentInformation;
using HousingResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.HousingResidentInformation;

namespace ResidentInformationApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static HousingResidentInformationResponse ToResponse(this HousingResidentInformation domain)
        {
            return new HousingResidentInformationResponse
            {
                // System = "academy", //TODO
                // SystemId = "123456", //TODO
                // SystemUrl = new System.Uri("https://academy-api.hackney.gov.uk"), //TODO
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                NhsNumber = domain.NhsNumber,
                DateOfBirth = domain.DateOfBirth,
                Uprn = domain.Uprn,
                AddressList = domain.AddressList?.ToResponse(),
                PhoneNumber = domain.PhoneNumberList?.ToResponse()
            };
        }
        public static List<HousingResidentInformationResponse> ToResponse(this IEnumerable<HousingResidentInformation> residents)
        {
            return residents.Select(r => r.ToResponse()).ToList();
        }

        private static List<Phone> ToResponse(this List<PhoneNumber> phoneNumbers)
        {
            return phoneNumbers.Select(number => new Phone
            {
                PhoneNumber = number.Number,
                PhoneType = number.Type
            }).ToList();
        }

        private static List<AddressResponse> ToResponse(this List<Address> addresses)
        {
            return addresses.Select(add => new AddressResponse
            {
                AddressLine1 = add.AddressLine1,
                AddressLine2 = add.AddressLine2,
                AddressLine3 = add.AddressLine3,
                PostCode = add.PostCode
            }).ToList();
        }
    }
}
