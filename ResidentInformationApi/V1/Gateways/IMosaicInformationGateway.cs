using System.Collections.Generic;
using System.Threading.Tasks;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IMosaicInformationGateway
    {
        string BaseAddress
        { get; }
        Task<List<MosaicResidentInformation>> GetResidentInformation(ResidentQueryParam rqp);
    }
}
