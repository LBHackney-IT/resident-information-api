using System.Collections.Generic;
using System.Threading.Tasks;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IAcademyInformationGateway
    {
        Task<List<AcademyClaimantInformation>> GetClaimantInformation(ResidentQueryParam rqp);
    }
}
