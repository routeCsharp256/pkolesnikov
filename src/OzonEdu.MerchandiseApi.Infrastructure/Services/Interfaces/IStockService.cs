using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces
{
    public interface IStockService
    {
        Task<bool> IsReadyToGiveOut(MerchDelivery delivery, CancellationToken token);
        Task<bool> TryGiveOutItems(MerchDelivery delivery, CancellationToken token);
    }
}