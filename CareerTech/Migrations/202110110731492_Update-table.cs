namespace CareerTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.About", "Main", c => c.Boolean(nullable: false));
            AddColumn("dbo.Recruitment", "Workingform", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Recruitment", "Amount", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Recruitment", "Experience", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Recruitment", "EndDate", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Recruitment", "DetailDesc", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.Introduction", "Main", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Recruitment", "Gender", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Subscription", "Period", c => c.Int(nullable: false));
            DropColumn("dbo.Recruitment", "Desc");
            DropColumn("dbo.Recruitment", "Requirement");
            DropColumn("dbo.Recruitment", "Benefit");
            DropColumn("dbo.Recruitment", "WorkTime");
            DropColumn("dbo.Recruitment", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recruitment", "Total", c => c.Int());
            AddColumn("dbo.Recruitment", "WorkTime", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Recruitment", "Benefit", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.Recruitment", "Requirement", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.Recruitment", "Desc", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Subscription", "Period", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Recruitment", "Gender", c => c.Boolean());
            DropColumn("dbo.Introduction", "Main");
            DropColumn("dbo.Recruitment", "DetailDesc");
            DropColumn("dbo.Recruitment", "EndDate");
            DropColumn("dbo.Recruitment", "Experience");
            DropColumn("dbo.Recruitment", "Amount");
            DropColumn("dbo.Recruitment", "Workingform");
            DropColumn("dbo.About", "Main");
        }
    }
}
