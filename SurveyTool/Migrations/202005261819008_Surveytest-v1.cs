namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Surveytestv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "Likes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "Likes");
        }
    }
}
