namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record IssuanceRequestViewModel
    {
        public long EmployeeId { get; set; } 
        public int MerchPackId { get; set; }
    }
}