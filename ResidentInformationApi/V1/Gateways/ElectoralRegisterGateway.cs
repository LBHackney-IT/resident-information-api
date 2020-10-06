using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResidentInformationApi.V1.Gateways
{
    public class ElectoralRegisterGateway : IElectoralRegisterGateway
    {
        private readonly HttpClient _client;
        public ElectoralRegisterGateway(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<ElectoralRegisterResidentInformation>> GetResidentsInformation(ResidentQueryParam rqp)
        {
            var rqpString = DictionaryBuilder.BuildQueryDictionary(rqp);
            var builder = new UriBuilder();
            builder.Query = rqpString;
            var response = await _client.GetAsync(new Uri("api/v1/residents" + builder.Query, UriKind.Relative)).ConfigureAwait(true);

            //throw exception if not 200
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<ElectoralRegisterResidentsResponse>(content);
            return results.Residents;
        }
    }
}
