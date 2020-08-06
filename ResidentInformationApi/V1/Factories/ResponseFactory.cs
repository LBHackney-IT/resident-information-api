using System;
using System.Collections.Generic;
using System.Linq;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using AcademyClaimantInformation = ResidentInformationApi.V1.Domain.AcademyClaimantInformation;
using AcademyClaimantInformationResponse = ResidentInformationApi.V1.Boundary.Responses.AcademyClaimantInformation;
using Address = ResidentInformationApi.V1.Domain.Address;
using AddressResponse = ResidentInformationApi.V1.Boundary.Responses.Address;
using Email = ResidentInformationApi.V1.Domain.Email;
using EmailResponse = ResidentInformationApi.V1.Boundary.Responses.Email;
using HousingResidentInformation = ResidentInformationApi.V1.Domain.HousingResidentInformation;
using HousingResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.HousingResidentInformation;
using MosaicResidentInformation = ResidentInformationApi.V1.Domain.MosaicResidentInformation;
using MosaicResidentInformationResponse = ResidentInformationApi.V1.Boundary.Responses.MosaicResidentInformation;
using PhoneType = ResidentInformationApi.V1.Domain.PhoneType;
using PhoneTypeResponse = ResidentInformationApi.V1.Boundary.Responses.PhoneType;

namespace ResidentInformationApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static AcademyClaimantInformationResponse ToResponse(this AcademyClaimantInformation claimants)
        {
            return new AcademyClaimantInformationResponse
            {
                ClaimId = claimants.ClaimId,
                PersonRef = claimants.PersonRef,
                FirstName = claimants.FirstName,
                LastName = claimants.LastName,
                DateOfBirth = claimants.DateOfBirth,
                NINumber = claimants.NINumber,
                ClaimantAddress = claimants.ClaimantAddress.ToResponse(),
                CheckDigit = claimants.CheckDigit
            };
        }

        public static HousingResidentInformationResponse ToResponse(this HousingResidentInformation domain)
        {
            return new HousingResidentInformationResponse
            {
                HouseReference = domain.HouseReference,
                PersonNumber = domain.PersonNumber,
                TenancyReference = domain.TenancyReference,
                NationalInsuranceNumber = domain.NationalInsuranceNumber,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                NhsNumber = domain.NhsNumber,
                DateOfBirth = domain.DateOfBirth,
                Uprn = domain.Uprn,
                Address = domain.Address?.ToResponse(),
                PhoneNumber = domain.PhoneNumberList?.ToResponse(),
                EmailAddressList = domain.EmailAddressList?.ToResponse()
            };
        }

        public static MosaicResidentInformationResponse ToResponse(this MosaicResidentInformation domain)
        {
            return new MosaicResidentInformationResponse
            {
                MosaicId = domain.MosaicId,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Uprn = domain.Uprn,
                DateOfBirth = domain.DateOfBirth,
                PhoneNumber = domain.PhoneNumberList?.ToResponse(),
                AddressList = domain.AddressList?.ToResponse(),
                NhsNumber = domain.NhsNumber
            };
        }

        private static List<Phone> ToResponse(this List<PhoneNumber> phoneNumbers)
        {
            return phoneNumbers.Select(r => r.ToResponse()).ToList();
        }

        private static Phone ToResponse(this PhoneNumber domain)
        {
            return new Phone
            {
                PhoneNumber = domain.Number,
                PhoneType = domain.Type.ToResponse()
            };
        }

        private static PhoneTypeResponse ToResponse(this PhoneType domain)
        {
            switch (domain)
            {
                case PhoneType.Primary:
                    return PhoneTypeResponse.Primary;
                case PhoneType.Home:
                    return PhoneTypeResponse.Home;
                case PhoneType.Mobile:
                    return PhoneTypeResponse.Mobile;
                case PhoneType.Fax:
                    return PhoneTypeResponse.Fax;
                default:
                    throw new ArgumentException("Phone type is invalid");
            }

        }

        private static List<AddressResponse> ToResponse(this List<Address> addresses)
        {
            return addresses.Select(add => add.ToResponse()).ToList();
        }

        private static AddressResponse ToResponse(this Address domain)
        {
            return new AddressResponse
            {
                AddressLine1 = domain.AddressLine1,
                AddressLine2 = domain.AddressLine2,
                AddressLine3 = domain.AddressLine3,
                PostCode = domain.PostCode
            };
        }

        private static List<EmailResponse> ToResponse(this List<Email> emails)
        {
            return emails.Select(r => r.ToResponse()).ToList();
        }
        private static EmailResponse ToResponse(this Email domain)
        {
            return new EmailResponse
            {
                EmailAddress = domain.EmailAddress,
                DateLastModified = domain.DateLastModified
            };
        }
    }
}
