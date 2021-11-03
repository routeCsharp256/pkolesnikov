using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.Commands
{
    public class CreateEmployeeCommand : IRequest
    {
        public string Name { get; set; }
        public int ClothingSizeId { get; set; }
        public string? EmailAddress { get; set; }
    }
}