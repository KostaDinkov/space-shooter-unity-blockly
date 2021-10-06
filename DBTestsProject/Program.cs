using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AzureSqlDbConnect;

namespace DBTestsProject
{
    class Program
    {
        private const string username = "kosta@kiberlab.net";
        static void Main(string[] args)
        {
            var dbApi = new DbApi(new GameDbContext());
            var problemStates = dbApi.GetAllProblemStates(username);
            var lastUnlockedProblem = dbApi.GetLastUnlockedProblem(username);
        }
    }
}