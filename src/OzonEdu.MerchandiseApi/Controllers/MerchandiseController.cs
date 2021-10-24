using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;

namespace OzonEdu.MerchandiseApi.Controllers
{
    [ApiController]
    [Route(RouteConstant.MerchRoute)]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> RequestMerch(CancellationToken token)
        {
            return Ok();
        }
    }
}