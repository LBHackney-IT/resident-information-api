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
    public class HousingInformationGateway : IHousingInformationGateway
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public HousingInformationGateway(HttpClient client)
        {
            _client = client;
            _baseUrl = Environment.GetEnvironmentVariable("HOUSING_API_ENDPOINT");
        }

        public async Task<List<HousingResidentInformation>> GetResidentInformation(ResidentQueryParam rqp)
        {
            var rqpString = DictionaryBuilder.BuildQueryDictionary(rqp);
            var builder = new UriBuilder(_baseUrl);
            builder.Query = rqpString;
            var response = await _client.PostAsync(builder.Uri, null).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<List<HousingResidentInformation>>(content);

            return results;
        }
    }
}
