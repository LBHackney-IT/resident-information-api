using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;

namespace ResidentInformationApi.V1.Gateways
{
    public class AcademyInformationGateway : IAcademyInformationGateway
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public AcademyInformationGateway(HttpClient client)
        {
            _client = client;
            _baseUrl = Environment.GetEnvironmentVariable("ACADEMY_API_ENDPOINT");
        }
        public async Task<List<AcademyClaimantInformation>> GetClaimantInformation(ResidentQueryParam rqp)
        {
            var rqpString = JsonConvert.SerializeObject(rqp);
            var response = await _client.PostAsync(_baseUrl, new StringContent(rqpString));
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<AcademyClaimantInformation>>(content);

            return results;
        }
    }
}