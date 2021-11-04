namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record CreateIssuanceRequestViewModel
    {
        public long EmployeeId { get; set; } 
        public int MerchPackId { get; set; }
        public bool IsManual { get; set; } = false;
    }
}