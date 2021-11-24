using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Commands;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.HttpModels;

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
        /// <param name="request">  </param>
        /// <param name="token">  </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GiveOutMerch([FromBody] GiveOutMerchRequest request, 
            CancellationToken token)
        {
            var command = new GiveOutMerchCommand
            {
                EmployeeId = request.EmployeeId,
                MerchPackTypeId = request.MerchPackTypeId,
                IsManual = true
            };
            
            await _mediator.Send(command, token);
            return Ok();
        }
        
        [HttpGet("delivery")]
        public async Task<ActionResult<string?>> GetMerchDeliveryStatus(
            [FromQuery] GetMerchDeliveryStatusRequest requestStatus, 
            CancellationToken token)
        {
            var query = new GetMerchDeliveryStatusQuery
            {
                EmployeeId = requestStatus.EmployeeId,
                MerchPackTypeId = requestStatus.MerchPackTypeId
            };
            
            var statusName = await _mediator.Send(query, token);
            return Ok(statusName);
        }
    }
}