using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace AzureSqlDbConnect
{
    public class DbApi
    {
        private GameDbContext db;

        //TODO replace with IGameDBContext - database server agnostic implementation
        public DbApi(GameDbContext db)
        {
            this.db = db;
        }

        public bool UserExists(string username)
        {
            var user = this.db.People.FirstOrDefault(p => p.Username == username);
            return user != null;
        }

        public Guid CreateUser(string username)
        {
            var person = new Person() {Username = username, Id = Guid.NewGuid()};
            this.db.People.Add(person);
            this.db.SaveChanges();
            return person.Id;
        }

        public void NewUserProblemStateInit(string username, Dictionary<string, List<string>> levels)
        {
            var user = this.db.People.FirstOrDefault(p => p.Username == username);

            var problemStates = new List<ProblemState>();
            foreach (var level in levels)
            {
                foreach (var problem in level.Value)
                {
                    var problemState = new ProblemState()
                    {
                        Id = Guid.NewGuid(),
                        Person = user,
                        LevelName = level.Key,
                        ProblemName = problem,
                        ProblemScore = 0,
                        ProblemBlocksXml = "",
                        ProblemLocked = true
                    };
                    problemStates.Add(problemState);
                }
            }

            this.db.ProblemStates.AddRange(problemStates);
            this.db.SaveChanges();
            
            // init new GameState
            var firstProblemState =
                this.db.ProblemStates.FirstOrDefault(p => p.LevelName == "l01" && p.ProblemName == "p01");
            var newGameState = new GameState()
            {
                Person = user,
                GameCompleted = false,
                GameStateId = Guid.NewGuid(),
                LastUnlockedProblem = firstProblemState
            };

            firstProblemState.ProblemLocked = false;
            user.GameState = newGameState;
            this.db.SaveChanges();

        }

        public ProblemState SaveProblemState(string username, string levelName, string problemName,
            string problemBlocksXml,
            int problemScore)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);

            //TODO initialize all problems at login
            if (problemState == null)
            {
                throw new ObjectNotFoundException("Could not find problemState");
            }

            problemState.ProblemBlocksXml = problemBlocksXml;
            problemState.ProblemScore = problemScore;

            this.db.SaveChanges();
            return problemState;
        }

        public void SetProblemLocked(string username, string levelName, string problemName, bool locked)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);

            if (problemState != null)
            {
                problemState.ProblemLocked = locked;
                this.db.SaveChanges();
                return;
            }
            throw new ObjectNotFoundException("Could not find problemState");
        }

        private ProblemState GetProblemState(string username, string levelName, string problemName)
        {
            var problemState = this.db.ProblemStates.FirstOrDefault(p => p.Person.Username == username
                                                                         && p.LevelName == levelName
                                                                         && p.ProblemName == problemName);
            return problemState;
        }

        public void SetProblemScore(string username, string levelName, string problemName, int score)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);
            problemState.ProblemScore = score;
            problemState.ProblemCompleted = true;
            this.db.SaveChangesAsync();
        }

        public SortedDictionary<string, List<ProblemState>> GetAllProblemStates(string username)
        {
            var dict=  this.db.ProblemStates.Where(p => p.Person.Username == username).GroupBy(group=>group.LevelName)
                .ToDictionary(group=>group.Key, group=>group.ToList().OrderBy(p=>p.ProblemName).ToList());
            return new SortedDictionary<string, List<ProblemState>>(dict);
        }

        public ProblemState GetLastUnlockedProblem(string username)
        {
            var user = this.db.People.FirstOrDefault(p => p.Username == username);
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }

            var result = this.db.GameStates.FirstOrDefault(gs => gs.GameStateId == user.Id).LastUnlockedProblem;
            return result;
        }

        public void SetLastUnlockedProblem(string username, Guid problemStateId)
        {
            this.db.GameStates.SingleOrDefault(gs => gs.Person.Username == username)
                .LastUnlockedProblem = this.db.ProblemStates.SingleOrDefault(ps => ps.Id == problemStateId);
            this.db.SaveChanges();
        }
    }
}
