using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public class MerchDelivery : Entity
    {
        public int? Id { get; set; }
        
        public int? MerchPackTypeId { get; set; }

        public int? MerchDeliveryStatusId { get; set; }
        
        public DateTime? StatusChangeDate { get; set; }
        
        public List<Employee> Employees { get; set; }
    }
}