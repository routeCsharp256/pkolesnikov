using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Temp
{
    [Migration(1)]
    public class MerchDeliveryStatuses : Migration
    {
        private const string TableName = "merch_delivery_statuses";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("alias").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}