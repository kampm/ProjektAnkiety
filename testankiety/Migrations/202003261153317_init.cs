namespace testankiety.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Survays",
                c => new
                    {
                        SurvayID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        UserAccRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurvayID)
                .ForeignKey("dbo.UserAccs", t => t.UserAccRefId, cascadeDelete: true)
                .Index(t => t.UserAccRefId);
            
            CreateTable(
                "dbo.MultipleChoiceQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Answer = c.Int(nullable: false),
                        Text = c.String(),
                        Type = c.Int(nullable: false),
                        Survay_SurvayID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survays", t => t.Survay_SurvayID)
                .Index(t => t.Survay_SurvayID);
            
            CreateTable(
                "dbo.MultipleChoiceAnswers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        Text = c.String(),
                        MultipleChoiceQuestion_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MultipleChoiceQuestions", t => t.MultipleChoiceQuestion_ID)
                .Index(t => t.MultipleChoiceQuestion_ID);
            
            CreateTable(
                "dbo.TextQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Answer = c.String(),
                        Text = c.String(),
                        Type = c.Int(nullable: false),
                        Survay_SurvayID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survays", t => t.Survay_SurvayID)
                .Index(t => t.Survay_SurvayID);
            
            CreateTable(
                "dbo.UserAccs",
                c => new
                    {
                        UserAccID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserAccID);
            
            CreateTable(
                "dbo.YesNoQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Answer = c.Boolean(nullable: false),
                        Text = c.String(),
                        Type = c.Int(nullable: false),
                        Survay_SurvayID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survays", t => t.Survay_SurvayID)
                .Index(t => t.Survay_SurvayID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.YesNoQuestions", "Survay_SurvayID", "dbo.Survays");
            DropForeignKey("dbo.Survays", "UserAccRefId", "dbo.UserAccs");
            DropForeignKey("dbo.TextQuestions", "Survay_SurvayID", "dbo.Survays");
            DropForeignKey("dbo.MultipleChoiceQuestions", "Survay_SurvayID", "dbo.Survays");
            DropForeignKey("dbo.MultipleChoiceAnswers", "MultipleChoiceQuestion_ID", "dbo.MultipleChoiceQuestions");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.YesNoQuestions", new[] { "Survay_SurvayID" });
            DropIndex("dbo.TextQuestions", new[] { "Survay_SurvayID" });
            DropIndex("dbo.MultipleChoiceAnswers", new[] { "MultipleChoiceQuestion_ID" });
            DropIndex("dbo.MultipleChoiceQuestions", new[] { "Survay_SurvayID" });
            DropIndex("dbo.Survays", new[] { "UserAccRefId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.YesNoQuestions");
            DropTable("dbo.UserAccs");
            DropTable("dbo.TextQuestions");
            DropTable("dbo.MultipleChoiceAnswers");
            DropTable("dbo.MultipleChoiceQuestions");
            DropTable("dbo.Survays");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
