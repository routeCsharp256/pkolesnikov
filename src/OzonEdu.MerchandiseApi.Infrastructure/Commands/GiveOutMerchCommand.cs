using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.Commands
{
    public class GiveOutMerchCommand : IRequest
    {
        public long EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }

        /// <summary>
        ///     Приходил ли самостоятельно сотрудник за мерчем.
        /// </summary>
        public bool IsManual { get; set; }
    }
}