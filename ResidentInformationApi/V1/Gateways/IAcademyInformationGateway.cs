using System.Collections.Generic;
using System.Threading.Tasks;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IAcademyInformationGateway
    {
        string BaseAddress
        { get; }
        Task<List<AcademyClaimantInformation>> GetClaimantInformation(ResidentQueryParam rqp);
    }
}

