using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace AzureSqlDbConnect
{
    public class GameDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public GameDbContext()
            : base(
                "Server=tcp:kiberlab-db-server.database.windows.net,1433;Initial Catalog=kiberlab-db;Persist Security Info=False;User ID=kiberlab-admin;Password=K05t4d1n7410!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
        {
        }


    }

    public class Person
    {

        public Guid Id { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }

        public string Email { get; set; }
    }
}
