
using AzureSqlDbConnect;
using System;

namespace DBTestsProject
{
    class Program
    {
        private const string username = "kosta@kiberlab.net";
        static void Main(string[] args)
        {
            var dbApi = new DbApi(new GameDbContext());
            
            if (!dbApi.UserExists(username))
            {
                var guid = dbApi.CreateUser(username);
                Console.WriteLine(guid);
            }
            Console.WriteLine(dbApi.GetConnectionString());
            var problemStates = dbApi.GetAllProblemStates(username);
            var lastUnlockedProblem = dbApi.GetLastUnlockedProblem(username);
        }
    }
}