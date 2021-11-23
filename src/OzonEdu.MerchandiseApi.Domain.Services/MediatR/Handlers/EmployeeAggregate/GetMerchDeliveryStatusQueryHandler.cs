using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Handlers.EmployeeAggregate
{
    public class GetMerchDeliveryStatusQueryHandler : IRequestHandler<GetMerchDeliveryStatusQuery, MerchDeliveryStatus?>
    {
        private readonly IEmployeeService _employeeService;

        public GetMerchDeliveryStatusQueryHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        
        public async Task<MerchDeliveryStatus?> Handle(GetMerchDeliveryStatusQuery request, CancellationToken token)
        {
            var employee = await _employeeService.FindAsync(request.EmployeeId, token);
            return employee?.MerchDeliveries
                .FirstOrDefault(d => d.MerchPackType.Id.Equals(request.MerchPackTypeId))?
                .Status;
        }
    }
}