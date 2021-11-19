using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;

namespace OzonEdu.MerchandiseApi.Domain.Services.Contracts.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetByIdAsync(int id, CancellationToken token = default)
        {
            var employee = await _employeeRepository.FindByIdAsync(id, token);
            if (employee is null)
                throw new Exception("employee not found");
            return employee;
        }

        public async Task<Employee?> GetByEmailAsync(string email, CancellationToken token)
        {
            var employee = await _employeeRepository.FindByEmailAsync(email, token);
            return employee;
        }

        public async Task<Employee> CreateAsync(string name, string email, CancellationToken token)
        {
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
            var updatedEmployee = await _employeeRepository.UpdateAsync(employeeForUpdate, token);
            if (updatedEmployee is null)
                throw new Exception("employee wasn't updated");
            return updatedEmployee;
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAsync(MerchDeliveryStatus status, 
            IEnumerable<long> suppliedSkuCollection, 
            CancellationToken token = default)
        {
            var statusId = MerchDeliveryStatus
                .GetAll<MerchDeliveryStatus>()
                .FirstOrDefault(s => s.Equals(status))?
                .Id;
            
            if (statusId is null)
                throw new Exception("Status not exists");
            
            return await _employeeRepository
                .GetByMerchDeliveryStatusAndSkuCollection(statusId.Value, suppliedSkuCollection, token);
        }
    }
}