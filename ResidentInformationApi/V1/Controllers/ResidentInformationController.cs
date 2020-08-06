using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.UseCase;

namespace ResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/residents")]
    [Produces("application/json")]
    public class ResidentInformationController : BaseController
    {
        private readonly IListContactsUseCase _listContactsUseCase;
        public ResidentInformationController(IListContactsUseCase listContactsUseCase)
        {
            _listContactsUseCase = listContactsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> ListContactsAsync([FromQuery] ResidentQueryParam rqp)
        {
            try
            {
                var response = await _listContactsUseCase.Execute(rqp).ConfigureAwait(true);
                return Ok(response);
            }
            catch (InvalidQueryParameterException e)
            {
                return BadRequest(e.Message);
            }
            catch (HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
