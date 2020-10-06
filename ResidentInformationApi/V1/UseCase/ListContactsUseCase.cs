using System;
using System.Linq;
using System.Threading.Tasks;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Factories;
using ResidentInformationApi.V1.Gateways;

namespace ResidentInformationApi.V1.UseCase
{
    public class ListContactsUseCase : IListContactsUseCase
    {
        private readonly IAcademyInformationGateway _academyGateway;
        private readonly IHousingInformationGateway _housingGateway;
        private readonly IMosaicInformationGateway _mosaicGateway;
        private readonly IElectoralRegisterGateway _electoralRegisterGateway;

        public ListContactsUseCase(
                        IAcademyInformationGateway academyGateway,
                        IHousingInformationGateway housingGateway,
                        IMosaicInformationGateway mosaicGateway,
                        IElectoralRegisterGateway electoralRegisterGateway
                        )
        {
            _academyGateway = academyGateway;
            _housingGateway = housingGateway;
            _mosaicGateway = mosaicGateway;
            _electoralRegisterGateway = electoralRegisterGateway;
        }

        public async Task<ResidentInformationResponse> Execute(ResidentQueryParam rqp)
        {
            var electoralRegisterRedsidents = _electoralRegisterGateway.GetResidentsInformation(rqp).Result;

            var academyClaimants = await _academyGateway.GetClaimantInformation(rqp).ConfigureAwait(true);
            var housingResidents = await _housingGateway.GetResidentInformation(rqp).ConfigureAwait(true);
            var mosaicResidents = await _mosaicGateway.GetResidentInformation(rqp).ConfigureAwait(true);
            var electoralRegisterResidents = await _electoralRegisterGateway.GetResidentsInformation(rqp).ConfigureAwait(true);

            var academyUrl = Environment.GetEnvironmentVariable("ACADEMY_API_URL");
            var housingUrl = Environment.GetEnvironmentVariable("HOUSING_API_URL");
            var mosaicUrl = Environment.GetEnvironmentVariable("MOSAIC_API_URL");
            var electoralRegisterUrl = Environment.GetEnvironmentVariable("ELECTORAL_REGISTER_API_URL");

            var academyResults = academyClaimants.Select(x =>
                new ResidentInformationResult
                {
                    System = "Academy",
                    SystemId = x.ClaimId.ToString(),
                    SystemUrl = new Uri(academyUrl + $"api/v1/claimants/claim/{x.ClaimId}/person/{x.PersonRef}"),
                    Data = x.ToResponse()
                });

            var housingResults = housingResidents.Select(x =>
                new ResidentInformationResult
                {
                    System = "Housing",
                    SystemId = x.HouseReference.ToString(),
                    SystemUrl = new Uri(housingUrl + $"api/v1/households/{x.HouseReference}/people/{x.PersonNumber}"),
                    Data = x.ToResponse()
                });

            var mosaicResults = mosaicResidents.Select(x =>
                new ResidentInformationResult
                {
                    System = "Mosaic",
                    SystemId = x.MosaicId.ToString(),
                    SystemUrl = new Uri(mosaicUrl + $"api/v1/residents/{x.MosaicId}"),
                    Data = x.ToResponse()
                });

            var electoralRegisterResults = electoralRegisterResidents.Select(x =>
                new ResidentInformationResult
                {
                    System = "Electoral Register",
                    SystemId = x.ElectoralRegisterId.ToString(),
                    SystemUrl = new Uri(electoralRegisterUrl + $"api/v1/residents/{x.ElectoralRegisterId}"),
                    Data = x.ToResponse()
                });
            var allResults = academyResults.Concat(housingResults.Concat(mosaicResults.Concat(electoralRegisterResults)));

            return new ResidentInformationResponse
            {
                Results = allResults.ToList()
            };
        }
    }
}
