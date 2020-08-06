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
using Phone = ResidentInformationApi.V1.Domain.Phone;
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
                NationalInsuranceNumber = domain.NiNumber,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                DateOfBirth = domain.DateOfBirth,
                Uprn = domain.Uprn,
                Address = domain.Address?.ToResponse(),
                PhoneNumber = domain.PhoneNumber?.ToResponse(),
                EmailAddressList = domain.Email?.ToResponse()
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
                PhoneNumber = domain.PhoneNumber?.ToResponse(),
                AddressList = domain.AddressList?.ToResponse(),
                NhsNumber = domain.NhsNumber
            };
        }

        private static List<Boundary.Responses.Phone> ToResponse(this List<Phone> phoneNumbers)
        {
            return phoneNumbers.Select(r => r.ToResponse()).ToList();
        }


        private static Boundary.Responses.Phone ToResponse(this Phone phoneNumber)
        {
            return new Boundary.Responses.Phone
            {
                PhoneNumber = phoneNumber.PhoneNumber,
                PhoneType = phoneNumber.PhoneType.ToResponse(),
            };
        }

        private static PhoneTypeResponse ToResponse(this string domain)
        {
            switch (domain)
            {
                case "Primary":
                    return PhoneTypeResponse.Primary;
                case "Home":
                    return PhoneTypeResponse.Home;
                case "Mobile":
                    return PhoneTypeResponse.Mobile;
                case "Fax":
                    return PhoneTypeResponse.Fax;
                case "Work":
                    return PhoneTypeResponse.Work;
                case "Unknown":
                    return PhoneTypeResponse.Unknown;
                default:
                    return PhoneTypeResponse.Unknown;
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

        private static List<EmailResponse> ToResponse(this List<HousingEmail> emails)
        {
            return emails.Select(r => r.ToResponse()).ToList();
        }
        private static EmailResponse ToResponse(this HousingEmail domain)
        {
            return new EmailResponse
            {
                EmailAddress = domain.EmailAddress,
                DateLastModified = domain.LastModified
            };
        }
    }
}
