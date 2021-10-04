namespace AzureSqlDbConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProblemCompletedToProblemState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemStates", "ProblemCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProblemStates", "ProblemCompleted");
        }
    }
}
