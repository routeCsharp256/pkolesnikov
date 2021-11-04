using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.Commands
{
    public class GiveOutMerchCommand : IRequest
    {
        public long EmployeeId { get; set; }
        public int MerchPackId { get; set; }

        public bool IsManual { get; set; } = false;
    }
}