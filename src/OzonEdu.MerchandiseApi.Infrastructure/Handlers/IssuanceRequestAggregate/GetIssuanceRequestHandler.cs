using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Queries.IssuanceRequestAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.IssuanceRequestAggregate
{
    public class GetIssuanceRequestHandler : IRequestHandler<GetIssuanceRequestStatusQuery, int>
    {
        private readonly IIssuanceRequestRepository _repository;

        public GetIssuanceRequestHandler(IIssuanceRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(GetIssuanceRequestStatusQuery request, CancellationToken token)
        {
            var result = await _repository
                .FindByEmployeeIdAndMerchPackIdIdAsync(request.EmployeeId, request.MerchPackId, token);
            return result.RequestStatus.Id;
        }
    }
}