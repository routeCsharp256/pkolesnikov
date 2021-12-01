using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Tracers;

namespace OzonEdu.MerchandiseApi.Infrastructure.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private const string ServiceName = nameof(EmployeeService);
        
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;
        private readonly CustomTracer _tracer;

        public EmployeeService(IEmployeeRepository employeeRepository, 
            IMerchDeliveryRepository merchDeliveryRepository,
            CustomTracer tracer)
        {
            _employeeRepository = employeeRepository;
            _merchDeliveryRepository = merchDeliveryRepository;
            _tracer = tracer;
        }

        public async Task<Employee?> FindAsync(int id, CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(FindAsync), ("filter", "id"));

            var employee = await _employeeRepository.FindAsync(id, token);

            if (employee is null)
                return employee;
            
            await AttachMerchDeliveries(employee, token);
            
            return employee;
        }

        public async Task<Employee?> FindAsync(string email, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(FindAsync), ("filter", "email"));
            
            var employee = await _employeeRepository.FindAsync(email, token);
            
            if (employee is null)
                return employee;
            
            await AttachMerchDeliveries(employee, token);
            
            return employee;
        }

        public async Task<Employee> CreateAsync(string name, string email, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(CreateAsync));
                
            var employeeData = new Employee(
                new Name(name),
                new EmailAddress(email));
                
            var employee = await _employeeRepository.CreateAsync(employeeData, token);
            if (employee is null)
                throw new Exception("employee wasn't created");

            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employeeForUpdate, CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(UpdateAsync));
            
            var updatedEmployee = await _employeeRepository.UpdateAsync(employeeForUpdate, token);
            if (updatedEmployee is null)
                throw new Exception("employee wasn't updated");
            return updatedEmployee;
        }

        public async Task AddMerchDelivery(int employeeId, int merchDeliveryId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(AddMerchDelivery));
            await _employeeRepository.AddMerchDelivery(employeeId, merchDeliveryId, token);
        }

        public async Task<IEnumerable<Employee>> GetAsync(MerchDeliveryStatus status, 
            IEnumerable<long> suppliedSkuCollection, 
            CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(GetAsync));

            var statusId = MerchDeliveryStatus
                .GetAll<MerchDeliveryStatus>()
                .FirstOrDefault(s => s.Equals(status))?
                .Id;
            
            if (statusId is null)
                throw new Exception("Status not exists");
            
            return await _employeeRepository
                .GetByMerchDeliveryStatusAndSkuCollection(statusId.Value, suppliedSkuCollection, token);
        }

        private async Task AttachMerchDeliveries(Employee employee, CancellationToken token)
        {
            var merchDeliveries = await _merchDeliveryRepository
                .GetAsync(employee.Id, token);

            if (merchDeliveries is null)
                return;
            
            foreach (var delivery in merchDeliveries)
                employee.AddMerchDelivery(delivery);
        }
    }
}