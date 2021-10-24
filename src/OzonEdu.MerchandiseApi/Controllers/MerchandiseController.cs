using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;

namespace OzonEdu.MerchandiseApi.Controllers
{
    [ApiController]
    [Route(RouteConstant.Route)]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        /// <summary>
        ///     Запросить мерч.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RequestMerch(CancellationToken token)
        {
            return Ok();
        }
    }
}