using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Temp
{
    [Migration(4)]
    public class MerchPackTypeMerchTypeMaps : Migration
    {
        private const string TableName = "merch_pack_type_merch_type_maps";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("merch_pack_type_id").AsInt32().NotNullable()
                .WithColumn("merch_type_id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}