namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record GetMerchDeliveryStatusViewModel
    {
        public long EmployeeId { get; set; } 
        public int MerchPackTypeId { get; set; }
    }
}