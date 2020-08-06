using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using ResidentInformationApi.V1.Boundary.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace ResidentInformationApi.Tests.V1.E2ETests
{
    public class ListResidents : IntegrationTests<Startup>
    {
        [Test]
        public async Task ListEndpointReturns200Response()
        {
            SetupAcademyResponseWithClaimants("[]");
            SetupHousingResponseWithClaimants("[]");
            SetupMosaicResponseWithClaimants("[]");
            var url = new Uri("/api/v1/residents?first_name=joe", UriKind.Relative);
            var response = await Client.GetAsync(url).ConfigureAwait(true);
            response.StatusCode.Should().Be(200);
        }

        private void SetupMosaicResponseWithClaimants(string claimants)
        {
            MockMosaicApi.Given(Request.Create().WithPath("/api/v1/residents").UsingGet())
                .RespondWith(Response.Create().WithBody(claimants, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        private void SetupAcademyResponseWithClaimants(string claimants)
        {
            MockAcademyAPI.Given(Request.Create().WithPath("/api/v1/claimants").UsingGet())
                .RespondWith(Response.Create().WithBody(claimants, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        private void SetupHousingResponseWithClaimants(string households)
        {
            MockHousingApi.Given(Request.Create().WithPath("/api/v1/households").UsingGet())
                .RespondWith(Response.Create().WithBody(households, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

        private static async Task<ResidentInformationResponse> DeserializeResponse(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var parseResponse = JsonConvert.DeserializeObject<ResidentInformationResponse>(stringContent);
            return parseResponse;
        }
    }
}
