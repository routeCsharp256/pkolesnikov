using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Services
{
    public interface ITraceableEmployeeService : IEmployeeService, ITraceable
    { }
}