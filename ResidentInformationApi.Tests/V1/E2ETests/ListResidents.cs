using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways;
using AcademyClaimantInformationResponse = ResidentInformationApi.V1.Boundary.Responses.AcademyClaimantInformation;

namespace ResidentInformationApi.Tests.V1.E2ETests
{
    public class ListResidents : IntegrationTests<Startup>
    {
        private readonly Fixture _fixture = new Fixture();
        [Test]
        public async Task ListEndpointReturns200Response()
        {
            HelperMethods.SetupAcademyResponseWithClaimants("{claimants: []}", MockAcademyAPI);
            HelperMethods.SetupHousingResponseWithHouseholds("{residents:[]}", MockHousingApi);
            HelperMethods.SetupMosaicResponseWithClaimants("{residents: []}", MockMosaicApi);
            var url = new Uri("/api/v1/residents?first_name=joe", UriKind.Relative);
            var response = await Client.GetAsync(url).ConfigureAwait(false);
            response.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task ListEndpointReturnsRecordsFromFromEachApi()
        {
            var expectedResponse = new ResidentInformationResponse();

            var claimants = _fixture.Create<AcademyClaimantResponse>();
            HelperMethods.SetupAcademyResponseWithClaimants(claimants.ToJson(), MockAcademyAPI);
            var expectedAcademyResponses = HelperMethods.MapToResponse(claimants);

            var households = _fixture.Create<HousingApiResponse>();
            HelperMethods.SetupHousingResponseWithHouseholds(households.ToJson(), MockHousingApi);
            var expectedHousingResponse = HelperMethods.MapToResponse(households);

            var residents = _fixture.Create<MosaicResidentResponse>();
            HelperMethods.SetupMosaicResponseWithClaimants(residents.ToJson(), MockMosaicApi);
            var expectedResidentsResponse = HelperMethods.MapToResponse(residents);

            expectedResponse.Results = expectedAcademyResponses.Concat(expectedHousingResponse).Concat(expectedResidentsResponse).ToList();

            var url = new Uri("/api/v1/residents?first_name=joe", UriKind.Relative);
            var response = await Client.GetAsync(url).ConfigureAwait(true);
            response.StatusCode.Should().Be(200);
            await AssertExpectedAndActualJsonTheSame(response, expectedResponse).ConfigureAwait(true);
        }

        private static async Task AssertExpectedAndActualJsonTheSame(HttpResponseMessage response,
            ResidentInformationResponse expectedResponse)
        {
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            stringContent.Should().BeEquivalentTo(expectedResponse.ToJson());
        }
    }
}
