namespace AzureSqlDbConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGameState : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameStates", "LastUnlockedProblem_Id", "dbo.ProblemStates");
            DropIndex("dbo.GameStates", new[] { "LastUnlockedProblem_Id" });
            RenameColumn(table: "dbo.GameStates", name: "LastUnlockedProblem_Id", newName: "LastUnlockedProblemId");
            AlterColumn("dbo.GameStates", "LastUnlockedProblemId", c => c.Guid(nullable: false));
            CreateIndex("dbo.GameStates", "LastUnlockedProblemId");
            AddForeignKey("dbo.GameStates", "LastUnlockedProblemId", "dbo.ProblemStates", "Id", cascadeDelete: true);
            DropColumn("dbo.GameStates", "ProblemStateGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GameStates", "ProblemStateGuid", c => c.Guid(nullable: false));
            DropForeignKey("dbo.GameStates", "LastUnlockedProblemId", "dbo.ProblemStates");
            DropIndex("dbo.GameStates", new[] { "LastUnlockedProblemId" });
            AlterColumn("dbo.GameStates", "LastUnlockedProblemId", c => c.Guid());
            RenameColumn(table: "dbo.GameStates", name: "LastUnlockedProblemId", newName: "LastUnlockedProblem_Id");
            CreateIndex("dbo.GameStates", "LastUnlockedProblem_Id");
            AddForeignKey("dbo.GameStates", "LastUnlockedProblem_Id", "dbo.ProblemStates", "Id");
        }
    }
}
