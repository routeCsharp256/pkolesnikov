using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;
#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchDeliveryRepository : IMerchDeliveryRepository
    {
        public MerchDeliveryRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }
        public async Task<MerchDelivery?> CreateAsync(MerchDelivery itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MerchDelivery?> UpdateAsync(MerchDelivery itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MerchDelivery?> FindByIdAsync(int id, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<MerchDelivery>> GetAll(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<MerchDelivery>> GetByStatus(int statusId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}