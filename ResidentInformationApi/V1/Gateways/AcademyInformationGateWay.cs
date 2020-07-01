using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Gateways.Helpers;

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
            var rqpString = DictionaryBuilder.BuildQueryDictionary(rqp);
            var builder = new UriBuilder(_baseUrl);
            builder.Query = rqpString;
            var response = await _client.PostAsync(builder.Uri, null).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<List<AcademyClaimantInformation>>(content);

            return results;
        }

        private static void NewMethod(ResidentQueryParam rqp, Dictionary<string, string> queryDictionary)
        {
            if (!string.IsNullOrEmpty(rqp.FirstName)) queryDictionary.Add("first_name", rqp.FirstName);
            if (!string.IsNullOrEmpty(rqp.LastName)) queryDictionary.Add("last_name", rqp.LastName);
            if (!string.IsNullOrEmpty(rqp.Address)) queryDictionary.Add("address", rqp.Address);
            if (!string.IsNullOrEmpty(rqp.Postcode)) queryDictionary.Add("postcode", rqp.Postcode);
        }
    }
}
