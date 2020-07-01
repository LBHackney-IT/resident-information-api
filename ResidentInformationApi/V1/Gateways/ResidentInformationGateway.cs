using System.Collections.Generic;
using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.V1.Gateways
{
    public class ResidentInformationGateway : IResidentInformationGateway
    {

        public ResidentInformation GetResidentInformationByAddress(string address)
        {
          // temporarily returning dummy data until the system api requests
          // are in place here
          var residentInformation = new ResidentInformation
          {
            FirstName = "First",
            LastName = "Last",
            Uprn = "123abc",
            DateOfBirth = "1980-10-02",
          };

          residentInformation.PhoneNumberList.Add(new PhoneNumber());
          residentInformation.AddressList.Add(new Address());

          return residentInformation;
        }
    }
}
