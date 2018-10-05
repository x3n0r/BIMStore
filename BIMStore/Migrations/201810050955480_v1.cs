namespace BIMStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_animals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 64, unicode: false),
                        species = c.String(maxLength: 64, unicode: false),
                        race = c.String(maxLength: 64, unicode: false),
                        date_of_birth = c.DateTime(),
                        notes = c.String(unicode: false),
                        cust_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(maxLength: 64, unicode: false),
                        description = c.String(unicode: false),
                        added_date = c.DateTime(nullable: false),
                        added_by = c.Int(),
                        tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_companydata",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 64, unicode: false),
                        slogan = c.String(maxLength: 64, unicode: false),
                        contact_phone = c.String(maxLength: 64, unicode: false),
                        contact_email = c.String(maxLength: 64, unicode: false),
                        IBAN = c.String(maxLength: 48, unicode: false),
                        BIC = c.String(maxLength: 48, unicode: false),
                        address_street = c.String(maxLength: 64, unicode: false),
                        address_postcode = c.String(maxLength: 64, unicode: false),
                        address_city = c.String(maxLength: 64, unicode: false),
                        address_country = c.String(maxLength: 64, unicode: false),
                        logo = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_dea_cust",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type = c.String(maxLength: 64, unicode: false),
                        first_name = c.String(maxLength: 128, unicode: false),
                        last_name = c.String(maxLength: 128, unicode: false),
                        form_of_address = c.String(maxLength: 64, unicode: false),
                        address_street = c.String(maxLength: 64, unicode: false),
                        address_postcode = c.String(maxLength: 64, unicode: false),
                        address_city = c.String(maxLength: 64, unicode: false),
                        address_country = c.String(maxLength: 64, unicode: false),
                        contact_phone = c.String(maxLength: 64, unicode: false),
                        contact_mail = c.String(maxLength: 64, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 64, unicode: false),
                        category = c.Int(),
                        description = c.String(unicode: false),
                        rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        added_date = c.DateTime(),
                        added_by = c.Int(),
                        hasqty = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_transaction_detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dea_cust_id = c.Int(),
                        added_date = c.DateTime(nullable: false),
                        added_by = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type = c.String(maxLength: 64, unicode: false),
                        dea_cust_id = c.Int(),
                        grandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        transaction_date = c.DateTime(nullable: false),
                        tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        added_by = c.Int(),
                        kontobez = c.String(maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        first_name = c.String(maxLength: 64, unicode: false),
                        last_name = c.String(maxLength: 64, unicode: false),
                        username = c.String(maxLength: 64, unicode: false),
                        password = c.String(maxLength: 64, unicode: false),
                        contact_phone = c.String(maxLength: 64, unicode: false),
                        contact_email = c.String(maxLength: 64, unicode: false),
                        address = c.String(maxLength: 64, unicode: false),
                        gender = c.String(maxLength: 32, unicode: false),
                        user_type = c.String(maxLength: 32, unicode: false),
                        added_date = c.DateTime(nullable: false),
                        added_by = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_users");
            DropTable("dbo.tbl_transactions");
            DropTable("dbo.tbl_transaction_detail");
            DropTable("dbo.tbl_products");
            DropTable("dbo.tbl_dea_cust");
            DropTable("dbo.tbl_companydata");
            DropTable("dbo.tbl_categories");
            DropTable("dbo.tbl_animals");
        }
    }
}
