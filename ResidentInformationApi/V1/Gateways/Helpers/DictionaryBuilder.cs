using System.Collections.Generic;
using System.Net.Http;
using ResidentInformationApi.V1.Boundary.Requests;

namespace ResidentInformationApi.V1.Gateways.Helpers
{
    public static class DictionaryBuilder
    {
        public static string BuildQueryDictionary(ResidentQueryParam rqp)
        {
            var queryDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(rqp.FirstName)) queryDictionary.Add("first_name", rqp.FirstName);
            if (!string.IsNullOrEmpty(rqp.LastName)) queryDictionary.Add("last_name", rqp.LastName);
            if (!string.IsNullOrEmpty(rqp.Address)) queryDictionary.Add("address", rqp.Address);
            if (!string.IsNullOrEmpty(rqp.Postcode)) queryDictionary.Add("postcode", rqp.Postcode);

            var rqpString = new FormUrlEncodedContent(queryDictionary).ReadAsStringAsync().Result;

            return rqpString;
        }
    }
}
