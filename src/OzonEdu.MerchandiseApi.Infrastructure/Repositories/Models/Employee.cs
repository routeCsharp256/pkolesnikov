using System.Collections.Generic;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public class Employee 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int? ClothingSizeId { get; set; }
        
        public string? EmailAddress { get; set; }

        public string? ManagerEmailAddress { get; set; }
        
        public List<MerchDelivery> MerchDeliveries { get; set; }
    }
}