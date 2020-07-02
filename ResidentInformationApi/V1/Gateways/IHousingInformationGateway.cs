using System.Collections.Generic;
using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IHousingInformationGateway
    {
        List<ResidentInformation> GetResidentInformationByAddress(string address);
    }
}
