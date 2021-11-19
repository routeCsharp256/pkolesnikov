using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> FindByIdAsync(int id, CancellationToken token = default);

        Task<Employee?> FindByEmailAsync(string email, CancellationToken token = default);
        
        Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAndSkuCollection(int statusId, 
            IEnumerable<long> skuIdCollection,
            CancellationToken token = default);
    }
}