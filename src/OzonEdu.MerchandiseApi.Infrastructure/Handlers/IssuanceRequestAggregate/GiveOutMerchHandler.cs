using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.IssuanceRequestAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private readonly IIssuanceRequestRepository _issuanceRequestRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GiveOutMerchHandler(IIssuanceRequestRepository issuanceRequestRepository,
            IEmployeeRepository employeeRepository,
            IMerchPackRepository merchPackRepository)
        {
            _employeeRepository = employeeRepository;
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

            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId, token);
            
            if (employee is null)
                throw new Exception("Employee not found");

            var isReady = true;
            foreach (var merchType in merchPackType.MerchTypes)
            {
                var size = merchType.HasSize
                    ? employee.ClothingSize
                    : null;
                //TODO Запрос количества товара по типу и размеру (если требуется),
                // выставить флаг isReady=false, если количество равно 0
            }

            var status = request.IsManual
                ? RequestStatus.WasArrival
                : RequestStatus.AutoPending;
            
            if (isReady)
            {
                if (issuanceRequest.RequestStatus.Id == GetDoneStatusId())
                    throw new Exception("Merch was issued");
            
                //TODO Зарезервировать мерч в stock-api
            
                if (issuanceRequest.RequestStatus.Id == GetDoneStatusId())
                    throw new Exception("Merch was issued");
                
                status = RequestStatus.Done;
            }

            issuanceRequest.SetRequestStatus(status);
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