namespace BIMStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblproducts_warningqty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_products", "warningqty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_products", "warningqty");
        }
    }
}
