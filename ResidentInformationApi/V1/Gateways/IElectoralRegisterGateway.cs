using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidentInformationApi.V1.Gateways
{
    public interface IElectoralRegisterGateway
    {
        Task<List<ElectoralRegisterResidentInformation>> GetResidentsInformation(ResidentQueryParam rqp);
    }
}
