
using System.Data.Entity;
using Models;


namespace AzureSqlDbConnect
{
    public class GameDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<ProblemState> ProblemStates { get; set; }
        public GameDbContext()
            : base(
                "Server=tcp:kiberlab-db-server.database.windows.net,1433;Initial Catalog=kiberlab-db;Persist Security Info=False;User ID=kiberlab-admin;Password=K05t4d1n7410!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
        {
        }

    }

    
}
