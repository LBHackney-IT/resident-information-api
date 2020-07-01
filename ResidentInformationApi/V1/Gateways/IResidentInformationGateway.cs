using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IResidentInformationGateway
    {
        ResidentInformation GetResidentInformationByAddress(string address);
    }
}
