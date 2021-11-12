using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;
#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }
        public async Task<Employee?> CreateAsync(Employee itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee?> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee?> FindByIdAsync(long id, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee?> FindByEmailAsync(string email, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee?> FindByDeliveryId(int deliveryId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatus(int statusId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}