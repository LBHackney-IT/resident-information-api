using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using AcademyClaimantInformationResponse = ResidentInformationApi.V1.Boundary.Responses.AcademyClaimantInformation;
using Address = ResidentInformationApi.V1.Boundary.Responses.Address;
using Email = ResidentInformationApi.V1.Boundary.Responses.Email;
using HousingResidentInformation = ResidentInformationApi.V1.Boundary.Responses.HousingResidentInformation;
using MosaicResidentInformation = ResidentInformationApi.V1.Boundary.Responses.MosaicResidentInformation;
using Phone = ResidentInformationApi.V1.Boundary.Responses.Phone;
using PhoneType = ResidentInformationApi.V1.Boundary.Responses.PhoneType;
using ResidentInformationApi.V1.Factories;

namespace ResidentInformationApi.Tests.V1.E2ETests
{
    public static class HelperMethods
    {
        public static string ToJson(this object toConvert)
        {
            return JsonConvert.SerializeObject(toConvert, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.None,
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
            });
        }

        public static void SetupMosaicResponseWithClaimants(string claimants, WireMockServer mockMosaicApi)
        {
            mockMosaicApi.Given(Request.Create().WithPath("/api/v1/residents").UsingGet())
                .RespondWith(Response.Create().WithBody(claimants, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        public static void SetupAcademyResponseWithClaimants(string claimants, WireMockServer mockAcademyApi)
        {
            mockAcademyApi.Given(Request.Create().WithPath("/api/v1/claimants").UsingGet())
                .RespondWith(Response.Create().WithBody(claimants, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        public static void SetupHousingResponseWithHouseholds(string households, WireMockServer mockHousingApi)
        {
            mockHousingApi.Given(Request.Create().WithPath("/api/v1/households").UsingGet())
                .RespondWith(Response.Create().WithBody(households, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        public static void SetupElectoralRegisterResponseWithResidents(string residents, WireMockServer mockHousingApi)
        {
            mockHousingApi.Given(Request.Create().WithPath("/api/v1/residents").UsingGet())
                .RespondWith(Response.Create().WithBody(residents, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        public static IEnumerable<ResidentInformationResult> MapToResponse(HousingApiResponse households)
        {
            return households.Residents.Select(h => new ResidentInformationResult
            {
                System = "Housing",
                SystemId = h.HouseReference,
                SystemUrl =
                    new Uri(
                        $"{Environment.GetEnvironmentVariable("HOUSING_API_URL")}api/v1/households/{h.HouseReference}/people/{h.PersonNumber}"),
                Data = new HousingResidentInformation
                {
                    Uprn = h.Uprn,
                    Address = new Address
                    {
                        AddressLine1 = h.Address.AddressLine1,
                        PostCode = h.Address.PostCode,
                    },
                    FirstName = h.FirstName,
                    HouseReference = h.HouseReference,
                    LastName = h.LastName,
                    NhsNumber = null,
                    NationalInsuranceNumber = h.NiNumber,
                    PersonNumber = h.PersonNumber.ToString(),
                    PhoneNumber = h.PhoneNumber.Select(p => new Phone
                    {
                        PhoneNumber = p.PhoneNumber,
                        PhoneType = p.PhoneType.ToResponse()
                    }).ToList(),
                    TenancyReference = h.TenancyReference,
                    DateOfBirth = h.DateOfBirth,
                    EmailAddressList = h.Email.Select(e => new Email
                    {
                        EmailAddress = e.EmailAddress,
                        DateLastModified = e.LastModified
                    }).ToList()
                }
            });
        }

        public static IEnumerable<ResidentInformationResult> MapToResponse(AcademyClaimantResponse claimants)
        {
            return claimants.Claimants.Select(c => new ResidentInformationResult
            {
                System = "Academy",
                SystemId = c.ClaimId.ToString(),
                SystemUrl = new Uri($"{Environment.GetEnvironmentVariable("ACADEMY_API_URL")}api/v1/claimants/claim/{c.ClaimId}/person/{c.PersonRef}"),
                Data = new AcademyClaimantInformationResponse
                {
                    CheckDigit = c.CheckDigit,
                    ClaimantAddress = new Address
                    {
                        AddressLine1 = c.ClaimantAddress.AddressLine1,
                        AddressLine2 = c.ClaimantAddress.AddressLine2,
                        AddressLine3 = c.ClaimantAddress.AddressLine3,
                        PostCode = c.ClaimantAddress.PostCode,
                    },
                    ClaimId = c.ClaimId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PersonRef = c.PersonRef,
                    DateOfBirth = c.DateOfBirth,
                    NINumber = c.NINumber
                }
            });
        }

        public static IEnumerable<ResidentInformationResult> MapToResponse(MosaicResidentResponse residents)
        {
            return residents.Residents.Select(r => new ResidentInformationResult
            {
                System = "Mosaic",
                SystemId = r.MosaicId,
                SystemUrl =
                    new Uri($"{Environment.GetEnvironmentVariable("MOSAIC_API_URL")}api/v1/residents/{r.MosaicId}"),
                Data = new MosaicResidentInformation
                {
                    Uprn = r.Uprn,
                    AddressList =
                        r.AddressList.Select(a => new Address
                        {
                            AddressLine1 = a.AddressLine1,
                            AddressLine2 = a.AddressLine2,
                            AddressLine3 = a.AddressLine3,
                            PostCode = a.PostCode
                        }).ToList(),
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    MosaicId = r.MosaicId,
                    NhsNumber = r.NhsNumber,
                    PhoneNumber =
                        r.PhoneNumber.Select(p => new ResidentInformationApi.V1.Boundary.Responses.Phone
                        {
                            PhoneNumber = p.PhoneNumber,
                            PhoneType = PhoneType.Unknown
                        }).ToList(),
                    DateOfBirth = r.DateOfBirth
                }
            });
        }

        public static IEnumerable<ResidentInformationResult> MapToResponse(ElectoralRegisterResidentsResponse residents)
        {
            return residents.Residents.Select(c => new ResidentInformationResult
            {
                System = "Electoral Register",
                SystemId = c.ElectoralRegisterId.ToString(),
                SystemUrl = new Uri($"{Environment.GetEnvironmentVariable("ELECTORAL_REGISTER_API_URL")}api/v1/residents/{c.ElectoralRegisterId}"),
                Data = new ElectoralRegisterResidentResponse
                {
                    ElectoralRegisterId = c.ElectoralRegisterId.ToString(),
                    DateOfBirth = c.DateOfBirth.Date.ToString(),
                    Title = c.Title,
                    FirstName = c.FirstName,
                    MiddleName = c.MiddleName,
                    LastName = c.LastName,
                    Uprn = c.Uprn,
                    Nationality = c.Nationality,
                    Email = c.Email
                }
            });
        }
    }
}
