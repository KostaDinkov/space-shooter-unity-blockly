namespace AzureSqlDbConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGameState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameStates",
                c => new
                    {
                        GameStateId = c.Guid(nullable: false),
                        GameCompleted = c.Boolean(nullable: false),
                        ProblemStateGuid = c.Guid(nullable: false),
                        LastUnlockedProblem_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.GameStateId)
                .ForeignKey("dbo.ProblemStates", t => t.LastUnlockedProblem_Id)
                .ForeignKey("dbo.People", t => t.GameStateId)
                .Index(t => t.GameStateId)
                .Index(t => t.LastUnlockedProblem_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameStates", "GameStateId", "dbo.People");
            DropForeignKey("dbo.GameStates", "LastUnlockedProblem_Id", "dbo.ProblemStates");
            DropIndex("dbo.GameStates", new[] { "LastUnlockedProblem_Id" });
            DropIndex("dbo.GameStates", new[] { "GameStateId" });
            DropTable("dbo.GameStates");
        }
    }
}
