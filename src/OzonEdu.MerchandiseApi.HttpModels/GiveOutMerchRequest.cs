namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record GiveOutMerchRequest
    {
        public long EmployeeId { get; set; } 
        public int MerchPackTypeId { get; set; }
        
        public bool IsManual { get; set; } = false;
    }
}