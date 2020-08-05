using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways.Helpers;

namespace ResidentInformationApi.V1.Gateways
{
    public class AcademyInformationGateway : IAcademyInformationGateway
    {
        private readonly HttpClient _client;
        //private readonly string _baseUrl;
        public string BaseAddress { get; private set; }

        public AcademyInformationGateway(HttpClient client)
        {
            _client = client;
            BaseAddress = _client.BaseAddress.OriginalString;
            //_baseUrl = Environment.GetEnvironmentVariable("ACADEMY_API_ENDPOINT");
        }
        public async Task<List<AcademyClaimantInformation>> GetClaimantInformation(ResidentQueryParam rqp)
        {
            var rqpString = DictionaryBuilder.BuildQueryDictionary(rqp);
            var builder = new UriBuilder();
            builder.Query = rqpString;
            var response = await _client.GetAsync(new Uri("api/v1/claimants" + builder.Query, UriKind.Relative)).ConfigureAwait(true);

            //throw exception if not 200
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<List<AcademyClaimantInformation>>(content);

            return results;
        }
    }
}
