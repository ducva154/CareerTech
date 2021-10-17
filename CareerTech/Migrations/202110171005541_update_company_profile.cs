namespace CareerTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_company_profile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyProfile", "Status", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyProfile", "Status");
        }
    }
}
