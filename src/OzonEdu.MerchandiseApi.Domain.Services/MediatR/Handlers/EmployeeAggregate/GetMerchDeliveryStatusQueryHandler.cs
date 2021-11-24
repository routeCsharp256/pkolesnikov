using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Handlers.EmployeeAggregate
{
    public class GetMerchDeliveryStatusQueryHandler : IRequestHandler<GetMerchDeliveryStatusQuery, string?>
    {
        private readonly IMerchService _merchService;

        public GetMerchDeliveryStatusQueryHandler(IMerchService merchService)
        {
            _merchService = merchService;
        }
        
        public async Task<string?> Handle(GetMerchDeliveryStatusQuery request, CancellationToken token)
        {
            var status = await _merchService
                .FindStatus(request.EmployeeId, request.MerchPackTypeId, token);
            return status?.Name;
        }
    }
}