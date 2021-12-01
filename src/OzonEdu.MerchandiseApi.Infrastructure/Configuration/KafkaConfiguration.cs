namespace OzonEdu.MerchandiseApi.Infrastructure.Configuration
{
    public class KafkaConfiguration
    {
        public string? BootstrapServers { get; set; }
        public string? StockReplenishedEventTopic { get; set; }
        public string? GroupId { get; set; }
    }
}