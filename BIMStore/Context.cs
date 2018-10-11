namespace BIMStore
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BIMStore.BLL;
    using BIMStore.Migrations;

    public partial class Context : DbContext
    {
        public Context()
            : base("Context")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        public virtual DbSet<tbl_animals> tbl_animals { get; set; }
        public virtual DbSet<tbl_categories> tbl_categories { get; set; }
        public virtual DbSet<tbl_companydata> tbl_companydata { get; set; }
        public virtual DbSet<tbl_dea_cust> tbl_dea_cust { get; set; }
        public virtual DbSet<tbl_products> tbl_products { get; set; }
        public virtual DbSet<tbl_transaction_detail> tbl_transaction_detail { get; set; }
        public virtual DbSet<tbl_transactions> tbl_transactions { get; set; }
        public virtual DbSet<tbl_users> tbl_users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_animals>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_animals>()
                .Property(e => e.species)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_animals>()
                .Property(e => e.race)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_animals>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_categories>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_categories>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.slogan)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.contact_phone)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.contact_email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.IBAN)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.BIC)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.address_street)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.address_postcode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.address_city)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.address_country)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_companydata>()
                .Property(e => e.logo)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.form_of_address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.address_street)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.address_postcode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.address_city)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.address_country)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.contact_phone)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dea_cust>()
                .Property(e => e.contact_mail)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_products>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_products>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_transactions>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_transactions>()
                .Property(e => e.kontobez)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.contact_email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.contact_phone)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.gender)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_users>()
                .Property(e => e.user_type)
                .IsUnicode(false);
        }
    }
}
