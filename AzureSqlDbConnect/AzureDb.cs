
using System.Data.Entity;
using Models;


namespace AzureSqlDbConnect
{
    public class GameDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<ProblemState> ProblemStates { get; set; }
        public DbSet<GameState> GameStates { get; set; }

        public GameDbContext()
            : base(
                "Server=workstation-new,1433;Database=SpaceShooter;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;User Id=space;Password=space")
        {
        }

    }

    
}
