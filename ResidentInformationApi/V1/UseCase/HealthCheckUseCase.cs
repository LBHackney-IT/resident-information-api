using Microsoft.Extensions.HealthChecks;
using ResidentInformationApi.V1.Boundary;

namespace ResidentInformationApi.V1.UseCase
{
    public class HealthCheckUseCase
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckUseCase(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        public HealthCheckResponse Execute()
        {
            var result = _healthCheckService.CheckHealthAsync().Result;

            var success = result.CheckStatus == CheckStatus.Healthy;
            return new HealthCheckResponse(success, result.Description);
        }
    }

}
