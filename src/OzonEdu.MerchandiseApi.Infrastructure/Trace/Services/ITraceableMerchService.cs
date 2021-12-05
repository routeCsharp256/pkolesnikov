using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Services
{
    public interface ITraceableMerchService : IMerchService, ITraceable
    { }
}