using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Queries.IssuanceRequestAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.EmployeeAggregate
{
    public class GetMerchDeliveryStatusQueryHandler : IRequestHandler<GetMerchDeliveryStatusQuery, MerchDeliveryStatus?>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetMerchDeliveryStatusQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        public async Task<MerchDeliveryStatus?> Handle(GetMerchDeliveryStatusQuery request, CancellationToken token)
        {
            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId, token);
            if (employee is null)
                throw new Exception("Employee not found by id");
            return employee
                .MerchDeliveries
                .FirstOrDefault(d => d.MerchPackType.Id.Equals(request.MerchPackTypeId))?
                .Status;
        }
    }
}