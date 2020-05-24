namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "User", c => c.String());
            AddColumn("dbo.Surveys", "Show", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "Show");
            DropColumn("dbo.Surveys", "User");
        }
    }
}
