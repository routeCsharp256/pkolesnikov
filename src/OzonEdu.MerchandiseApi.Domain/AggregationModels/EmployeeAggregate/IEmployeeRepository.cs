using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> FindByIdAsync(long id, CancellationToken token = default);

        Task<Employee?> FindByEmailAsync(string email, CancellationToken token = default);

        Task<Employee?> FindByDeliveryId(int deliveryId, CancellationToken token = default);
        
        Task<IEnumerable<Employee>> GetByMerchDeliveryStatus(int statusId, CancellationToken token = default);
    }
}