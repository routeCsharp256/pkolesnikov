using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.DomainEvent
{
    public class ReplenishmentDomainEventHandler : INotificationHandler<ReplenishmentDomainEvent>
    {
        private readonly IIssuanceRequestRepository _issuanceRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IMediator _mediator;

        public ReplenishmentDomainEventHandler(IIssuanceRequestRepository issuanceRequestRepository,
            IEmployeeRepository employeeRepository,
            IMerchPackRepository merchPackRepository,
            IMediator mediator)
        {
            _issuanceRequestRepository = issuanceRequestRepository;
            _employeeRepository = employeeRepository;
            _merchPackRepository = merchPackRepository;
            _mediator = mediator;
        }

        public async Task Handle(ReplenishmentDomainEvent notification, CancellationToken token)
        {
            var manualRequests = await _issuanceRequestRepository
                .GetAllByStatusAsync(RequestStatus.WasArrival.Id, token);

            var autoRequests = await _issuanceRequestRepository
                .GetAllByStatusAsync(RequestStatus.AutoPending.Id, token);

            var requests = new List<IssuanceRequest>(manualRequests);
            requests.AddRange(autoRequests);

            var merchPacks = await _merchPackRepository.GetAll(token);

            var properRequests = requests
                .Select(r => new
                {
                    RequestId = r.RequestNumber,
                    RequestStatus = r.MerchPackStatus,
                    EmployeeId = r.EmployeeId,
                    MerchPackId = merchPacks
                        .First(p => p.MerchPackId.Equals(r.MerchPackId))
                        .Id
                })
                .Where(x => notification
                    .MerchTypes
                    .Select(t => t.Id)
                    .Contains(x.MerchPackId));
            
            foreach (var request in properRequests)
            {
                var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId.Value, token);
                if (employee is null)
                {
                    //TODO логируем или как-то иначе обрабатываем, что пользователь не найден
                    continue;
                }

                if (request.RequestStatus == RequestStatus.WasArrival)
                {
                    var email = employee.EmailAddress.Value;
                    //TODO отправляем уведомление на почту сотрудника, что появился мерч
                    continue;
                }
                
                var command = new GiveOutMerchCommand
                {
                    EmployeeId = request.EmployeeId.Value,
                    MerchPackId = request.MerchPackId,
                    IsManual = false
                };

                await _mediator.Send(command, token);
            }
        }
    }
}