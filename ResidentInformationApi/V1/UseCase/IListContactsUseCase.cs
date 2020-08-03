using System.Threading.Tasks;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;

namespace ResidentInformationApi.V1.UseCase
{
    public interface IListContactsUseCase
    {
        Task<ResidentInformationResponse> Execute(ResidentQueryParam rqp);
    }
}
