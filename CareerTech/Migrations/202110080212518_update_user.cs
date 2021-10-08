namespace CareerTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "FullName", c => c.String(maxLength: 255, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "FullName");
        }
    }
}
