using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.HttpModels;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.Queries.IssuanceRequestAggregate;

namespace OzonEdu.MerchandiseApi.Controllers
{
    [ApiController]
    [Route(RouteConstant.Route)]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        ///     Запросить мерч.
        /// </summary>
        /// <param name="requestStatus">  </param>
        /// <param name="token">  </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GiveOutMerch([FromQuery] CreateIssuanceRequestViewModel requestStatus, 
            CancellationToken token)
        {
            var command = new GiveOutMerchCommand
            {
                EmployeeId = requestStatus.EmployeeId,
                MerchPackId = requestStatus.MerchPackId,
                IsManual = true
            };
            try
            {
                await _mediator.Send(command, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        
        [HttpGet("issuance")]
        public async Task<ActionResult<string>> GetMerchIssuance([FromQuery] GetIssuanceRequestStatusViewModel requestStatus, 
            CancellationToken token)
        {
            var query = new GetIssuanceRequestStatusQuery
            {
                EmployeeId = requestStatus.EmployeeId,
                MerchPackId = requestStatus.MerchPackId
            };
            try
            {
                var statusName = await _mediator.Send(query, token);
                return Ok(statusName);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}