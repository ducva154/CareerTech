namespace CareerTech.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.About",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Title = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Detail = c.String(nullable: false, unicode: false),
                        Main = c.Boolean(nullable: false),
                        Desc = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        FullName = c.String(maxLength: 255, storeType: "nvarchar"),
                        Url_Image = c.String(maxLength: 255, unicode: false),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        DateApply = c.DateTime(nullable: false, storeType: "date"),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RecruitmentID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Status = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recruitment", t => t.RecruitmentID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.RecruitmentID);
            
            CreateTable(
                "dbo.Recruitment",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        CompanyProfileID = c.String(nullable: false, maxLength: 255, unicode: false),
                        JobID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Title = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Address = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Salary = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Workingform = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Amount = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Position = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Experience = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        Gender = c.Boolean(),
                        DetailDesc = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyProfileID)
                .ForeignKey("dbo.Job", t => t.JobID)
                .Index(t => t.CompanyProfileID)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.CompanyProfile",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        CompanyName = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Desc = c.String(nullable: false, unicode: false),
                        Address = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 255, unicode: false),
                        Phone = c.String(nullable: false, maxLength: 50, unicode: false),
                        Url_Avatar = c.String(maxLength: 255, unicode: false),
                        Url_Background = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        JobName = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Introduction",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Title = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Detail = c.String(nullable: false, unicode: false),
                        Main = c.Boolean(nullable: false),
                        Url_Image = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        SubscriptionID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        OrderDate = c.DateTime(nullable: false, storeType: "date"),
                        TotalPrice = c.Double(nullable: false),
                        Status = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Subscription", t => t.SubscriptionID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.SubscriptionID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        OrderID = c.String(nullable: false, maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(maxLength: 255, unicode: false),
                        Price = c.Single(),
                        Type = c.String(maxLength: 255, unicode: false),
                        Period = c.Int(),
                        DetailDesc = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Portfolio",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        PublicStatus = c.Boolean(nullable: false),
                        Url_Domain = c.String(nullable: false, maxLength: 255, unicode: false),
                        MainStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Education",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        PortfolioID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Time = c.String(nullable: false, maxLength: 255, unicode: false),
                        Detail = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Portfolio", t => t.PortfolioID, cascadeDelete: true)
                .Index(t => t.PortfolioID);
            
            CreateTable(
                "dbo.Experience",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        PortfolioID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Time = c.String(nullable: false, maxLength: 255, unicode: false),
                        Detail = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Portfolio", t => t.PortfolioID, cascadeDelete: true)
                .Index(t => t.PortfolioID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        PortfolioID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Url_Image = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Skill = c.String(maxLength: 255, storeType: "nvarchar"),
                        Domain = c.String(maxLength: 255, unicode: false),
                        TeamSize = c.Int(),
                        ProjectTech = c.String(maxLength: 255, storeType: "nvarchar"),
                        WorkProces = c.String(maxLength: 255, storeType: "nvarchar"),
                        Company = c.String(maxLength: 255, storeType: "nvarchar"),
                        ProjectRole = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Portfolio", t => t.PortfolioID, cascadeDelete: true)
                .Index(t => t.PortfolioID);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        PortfolioID = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Position = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Desc = c.String(unicode: false),
                        Address = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Age = c.Int(),
                        Gender = c.Boolean(),
                        Phone = c.String(nullable: false, maxLength: 255, unicode: false),
                        Email = c.String(nullable: false, maxLength: 255, unicode: false),
                        Url_avatar = c.String(maxLength: 255, unicode: false),
                        Instagram_url = c.String(maxLength: 255, unicode: false),
                        Facebook_url = c.String(maxLength: 255, unicode: false),
                        Twitter_url = c.String(maxLength: 255, unicode: false),
                        Youtube_url = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Portfolio", t => t.PortfolioID, cascadeDelete: true)
                .Index(t => t.PortfolioID);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        PortfolioID = c.String(nullable: false, maxLength: 255, unicode: false),
                        SkillName = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        SkillLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Portfolio", t => t.PortfolioID, cascadeDelete: true)
                .Index(t => t.PortfolioID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RoleId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Solution",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Title = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Detail = c.String(nullable: false, unicode: false),
                        Url_image = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Time",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 255, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Time", "UserID", "dbo.User");
            DropForeignKey("dbo.Solution", "UserID", "dbo.User");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.Portfolio", "UserID", "dbo.User");
            DropForeignKey("dbo.Skills", "PortfolioID", "dbo.Portfolio");
            DropForeignKey("dbo.Profile", "PortfolioID", "dbo.Portfolio");
            DropForeignKey("dbo.Product", "PortfolioID", "dbo.Portfolio");
            DropForeignKey("dbo.Experience", "PortfolioID", "dbo.Portfolio");
            DropForeignKey("dbo.Education", "PortfolioID", "dbo.Portfolio");
            DropForeignKey("dbo.Order", "UserID", "dbo.User");
            DropForeignKey("dbo.Order", "SubscriptionID", "dbo.Subscription");
            DropForeignKey("dbo.Payment", "OrderID", "dbo.Order");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.Introduction", "UserID", "dbo.User");
            DropForeignKey("dbo.CompanyProfile", "UserID", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.Candidates", "UserID", "dbo.User");
            DropForeignKey("dbo.Recruitment", "JobID", "dbo.Job");
            DropForeignKey("dbo.Recruitment", "CompanyProfileID", "dbo.CompanyProfile");
            DropForeignKey("dbo.Candidates", "RecruitmentID", "dbo.Recruitment");
            DropForeignKey("dbo.About", "UserID", "dbo.User");
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.Time", new[] { "UserID" });
            DropIndex("dbo.Solution", new[] { "UserID" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Skills", new[] { "PortfolioID" });
            DropIndex("dbo.Profile", new[] { "PortfolioID" });
            DropIndex("dbo.Product", new[] { "PortfolioID" });
            DropIndex("dbo.Experience", new[] { "PortfolioID" });
            DropIndex("dbo.Education", new[] { "PortfolioID" });
            DropIndex("dbo.Portfolio", new[] { "UserID" });
            DropIndex("dbo.Payment", new[] { "OrderID" });
            DropIndex("dbo.Order", new[] { "UserID" });
            DropIndex("dbo.Order", new[] { "SubscriptionID" });
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            DropIndex("dbo.Introduction", new[] { "UserID" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropIndex("dbo.CompanyProfile", new[] { "UserID" });
            DropIndex("dbo.Recruitment", new[] { "JobID" });
            DropIndex("dbo.Recruitment", new[] { "CompanyProfileID" });
            DropIndex("dbo.Candidates", new[] { "RecruitmentID" });
            DropIndex("dbo.Candidates", new[] { "UserID" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.About", new[] { "UserID" });
            DropTable("dbo.Role");
            DropTable("dbo.Time");
            DropTable("dbo.Solution");
            DropTable("dbo.UserRole");
            DropTable("dbo.Skills");
            DropTable("dbo.Profile");
            DropTable("dbo.Product");
            DropTable("dbo.Experience");
            DropTable("dbo.Education");
            DropTable("dbo.Portfolio");
            DropTable("dbo.Subscription");
            DropTable("dbo.Payment");
            DropTable("dbo.Order");
            DropTable("dbo.UserLogin");
            DropTable("dbo.Introduction");
            DropTable("dbo.UserClaim");
            DropTable("dbo.Job");
            DropTable("dbo.CompanyProfile");
            DropTable("dbo.Recruitment");
            DropTable("dbo.Candidates");
            DropTable("dbo.User");
            DropTable("dbo.About");
        }
    }
}
