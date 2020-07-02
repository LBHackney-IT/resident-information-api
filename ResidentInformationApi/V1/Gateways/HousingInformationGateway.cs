using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ResidentInformationApi.V1.Domain;
using Newtonsoft.Json;
// using System.Linq;

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

        public async Task<List<ResidentInformation>> GetResidentInformationByAddress(string address)
        {
          // ASSUMPTION: we can pass along the received address search parameter
          // straight to the Housing Information API and get the correct response

            if (address.Length == 0)
            {
                return new List<ResidentInformation>();
            }
            else
            {
                var response = await _client.PostAsync(_baseUrl, new StringContent(address));
                var content = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject(content);

                //TODO where to convert to domain objects?
                // var result = results.Select(result => x + 3);

                // var residentInformation = new ResidentInformation
                // {
                //     HouseReference = "house_ref",
                //     PersonNumber = "person_number",
                //     TenancyReference = "ten_ref",
                //     FirstName = "First",
                //     LastName = "Last",
                //     DateOfBirth = "1980-10-02",
                //     NationalInsuranceNumber = "12-34-56-78",
                //     Uprn = "123abc",
                //     PhoneNumberList = new List<PhoneNumber> { new PhoneNumber() },
                //     EmailAddressList = new List<Email> { new Email() },
                //     AddressList = new List<Address> { new Address() },
                // };

                return new List<ResidentInformation> { residentInformation };
            };

        }
    }
}

          //TODO make real api request e.g. get /households/address=address
          // example response:
          // [
          //   {
          //     "houseReference": "string",
          //     "personNumber": "string",
          //     "tenancyReference": "string",
          //     "firstName": "string",
          //     "lastName": "string",
          //     "dateOfBirth": "2020-07-02",
          //     "nationalInsuranceNumber": "string",
          //     "uprn": "string",
          //     "phoneNumbers": [
          //       {
          //         "phoneNumber": "07123456789",
          //         "phoneType": "Mobile",
          //         "dateLastModified": "2020-07-02T14:33:34.805Z"
          //       }
          //     ],
          //     "emailAddresses": [
          //       {
          //         "emailAddress": "user@example.com",
          //         "dateLastModified": "2020-07-02T14:33:34.805Z"
          //       }
          //     ],
          //     "address": {
          //       "propertyReference": "string",
          //       "addressLine1": "1 Hillman Street",
          //       "postCode": "E8 1DY"
          //     }
          //   }
          // ]
