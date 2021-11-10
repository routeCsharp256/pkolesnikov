using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces
{
    public interface IMerchService
    {
        Task<MerchDelivery> CreateMerchDeliveryAsync(MerchType merchType,
            ClothingSize? size,
            CancellationToken token = default);
    }
}