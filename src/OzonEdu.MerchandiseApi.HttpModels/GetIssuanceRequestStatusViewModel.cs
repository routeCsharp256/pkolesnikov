namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record GetIssuanceRequestStatusViewModel
    {
        public long EmployeeId { get; set; } 
        public int MerchPackId { get; set; }
    }
}