using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.Commands
{
    public class CreateEmployeeCommand : IRequest
    {
        public string name { get; set; }
        public int clothingSizeId { get; set; }
        public string? emailAddress { get; set; }
    }
}