using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.IssuanceRequestAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private readonly IMerchItemRepository _merchItemRepository;
        private readonly IIssuanceRequestRepository _issuanceRequestRepository;
        private readonly IMerchPackRepository _merchPackRepository;

        public GiveOutMerchHandler(IMerchItemRepository merchItemRepository,
            IIssuanceRequestRepository issuanceRequestRepository,
            IEmployeeRepository employeeRepository,
            IMerchPackRepository merchPackRepository)
        {
            _merchItemRepository = merchItemRepository;
            _issuanceRequestRepository = issuanceRequestRepository;
            _merchPackRepository = merchPackRepository;
        }
        
        public async Task<Unit> Handle(GiveOutMerchCommand request, CancellationToken token)
        {
            var issuanceRequest = await _issuanceRequestRepository
                .FindByEmployeeIdAndMerchPackIdIdAsync(request.EmployeeId, request.MerchPackId, token);
            if (issuanceRequest is null)
            {
                var newIssuanceRequest = new IssuanceRequest(
                    new EmployeeId(request.EmployeeId),
                    new MerchPackId(request.MerchPackId));
                issuanceRequest = await _issuanceRequestRepository.CreateAsync(newIssuanceRequest, token);
                await _issuanceRequestRepository.UnitOfWork.SaveEntitiesAsync(token);
            }

            if (issuanceRequest.RequestStatus.Id == GetDoneStatusId())
                throw new Exception("Merch was issued");

            var merchPack = await _merchPackRepository.FindByIdAsync(request.MerchPackId, token);

            if (merchPack is null)
                throw new Exception("Merch pack not found");

            var merchPackType = MerchPackType
                .GetAll<MerchPackType>()
                .FirstOrDefault(x => x.Id == merchPack.MerchPackType.Id);

            if (merchPackType is null)
                throw new Exception("Merch pack type not found");

            var isReady = true;
            foreach (var itemId in  merchPackType.MerchItemIds)
            {
                var item = await _merchItemRepository.FindByIdAsync(itemId.Value, token);
                //TODO Запрос количества токара по item.Sku, выставить флаг isReady=false, если количество равно 0
            }
            
            if (issuanceRequest.RequestStatus.Id == GetDoneStatusId())
                throw new Exception("Merch was issued");
            
            //TODO Зарезервировать мерч в stock-api
            
            if (issuanceRequest.RequestStatus.Id == GetDoneStatusId())
                throw new Exception("Merch was issued");
            
            issuanceRequest.SetRequestStatus(RequestStatus.Done);

            await _issuanceRequestRepository.UpdateAsync(issuanceRequest, token);
            await _issuanceRequestRepository.UnitOfWork.SaveEntitiesAsync(token);
            
            return Unit.Value;
        }

        private static int GetDoneStatusId()
        {
            var doneStatus = RequestStatus
                .GetAll<RequestStatus>()
                .FirstOrDefault(x => x.Equals(RequestStatus.Done));
            if (doneStatus is null)
                throw new Exception("Unknown status of issuance request");
            return doneStatus.Id;
        }
    }
}