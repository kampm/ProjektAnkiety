namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abcd_test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "ABCDQuestions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "ABCDQuestions");
        }
    }
}
