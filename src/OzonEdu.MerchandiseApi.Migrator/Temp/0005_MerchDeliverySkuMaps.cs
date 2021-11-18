using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Temp
{
    [Migration(5)]
    public class MerchDeliverySkuMaps : Migration
    {
        private const string TableName = "merch_delivery_sku_maps";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("merch_delivery_id").AsInt32().NotNullable()
                .WithColumn("sku_id").AsInt64().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}