namespace AzureSqlDbConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProblemStates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProblemStates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PersonId = c.Guid(nullable: false),
                        LevelName = c.String(),
                        ProblemName = c.String(),
                        ProblemBlocksXml = c.String(),
                        ProblemScore = c.Int(nullable: false),
                        ProblemLocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProblemStates", "PersonId", "dbo.People");
            DropIndex("dbo.ProblemStates", new[] { "PersonId" });
            DropTable("dbo.ProblemStates");
        }
    }
}
