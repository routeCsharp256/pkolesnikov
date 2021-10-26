using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.HttpModels;
#pragma warning disable 1591

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
        /// <param name="request">  </param>
        /// <param name="token">  </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetMerchResponse>> GetMerch(GetMerchRequest request, CancellationToken token)
        {
            return await Task.Run(() => Ok(new GetMerchResponse()), token);
        }
        
        [HttpGet("issuance")]
        public async Task<ActionResult<GetMerchIssuanceResponse>> GetMerchIssuance(GetMerchIssuanceRequest request, 
            CancellationToken token)
        {
            return await Task.Run(() => Ok(new GetMerchIssuanceResponse()), token);
        }
    }
}