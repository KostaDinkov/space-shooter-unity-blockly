namespace AzureSqlDbConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPersonUsername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Username");
        }
    }
}
