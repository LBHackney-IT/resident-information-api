using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;

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

        public async Task<List<ResidentInformation>> GetResidentInformation(ResidentQueryParam rqp)
        {
            var rqpString = JsonConvert.SerializeObject(rqp);
            var response = await _client.PostAsync(_baseUrl, new StringContent(rqpString));
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<ResidentInformation>>(content);

            Console.WriteLine(results.GetType().ToString());
            return results;
        }
    }
}
