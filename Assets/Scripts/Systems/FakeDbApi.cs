using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems
{
    public class FakeDbApi
    {

        private List<Person> people;

        //TODO replace with IGameDBContext - database server agnostic implementation
        public FakeDbApi()
        {
            this.people = new List<Person>();
        }

        public Person GetUser(string username)
        {
            return people.FirstOrDefault(p => p.Username == username);
        }



        public bool UserExists(string username)
        {

            var user = people.FirstOrDefault(p => p.Username == username);
            return user != null;
        }

        public Guid CreateUser(string username)
        {
            var person = new Person() { Username = username, Id = Guid.NewGuid() };
            this.people.Add(person);
            return person.Id;
        }

        public void NewUserProblemStateInit(string username, Dictionary<string, List<string>> levels)
        {
            var user = this.people.FirstOrDefault(p => p.Username == username);

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

            user.ProblemStates.AddRange(problemStates);


            // init new GameState
            var firstProblemState =
               user.ProblemStates.FirstOrDefault(p => p.LevelName == "l01" && p.ProblemName == "p01");

            var newGameState = new GameState()
            {
                Person = user,
                GameCompleted = false,

                LastUnlockedProblem = firstProblemState
            };

            firstProblemState.ProblemLocked = false;
            user.GameState = newGameState;


        }

        public ProblemState SaveProblemState(string username, string levelName, string problemName,
            string problemBlocksXml,
            int problemScore)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);



            problemState.ProblemBlocksXml = problemBlocksXml;
            problemState.ProblemScore = problemScore;


            return problemState;
        }

        public void SetProblemLocked(string username, string levelName, string problemName, bool locked)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);

            if (problemState != null)
            {
                problemState.ProblemLocked = locked;

                return;
            }

        }

        private ProblemState GetProblemState(string username, string levelName, string problemName)
        {
            var user = people.FirstOrDefault(p => p.Username == username);
            var problemState = user.ProblemStates.FirstOrDefault(p => p.Person.Username == username
                                                                         && p.LevelName == levelName
                                                                         && p.ProblemName == problemName);
            return problemState;
        }

        public void SetProblemScore(string username, string levelName, string problemName, int score)
        {
            var problemState = this.GetProblemState(username, levelName, problemName);
            problemState.ProblemScore = score;
            problemState.ProblemCompleted = true;

        }

        public SortedDictionary<string, List<ProblemState>> GetAllProblemStates(string username)
        {
            var user = this.people.FirstOrDefault(p => p.Username == username);
            var dict = user.ProblemStates.GroupBy(group => group.LevelName)
                .ToDictionary(group => group.Key, group => group.ToList().OrderBy(p => p.ProblemName).ToList());
            return new SortedDictionary<string, List<ProblemState>>(dict);
        }

        public ProblemState GetLastUnlockedProblem(string username)
        {
            var user = people.FirstOrDefault(p => p.Username == username);
            if (user == null)
            {
                throw new ArgumentException($"User [{username}] not found");
            }

            var gState = user.GameState.LastUnlockedProblem;
            if (gState == null)
            {
                throw new ArgumentException($"No GameState for {user.Username}");
            }

            return gState;
        }

        public void SetLastUnlockedProblem(string username, Guid problemStateId)
        {
            var user = people.FirstOrDefault(p => p.Username == username);
            user.GameState.LastUnlockedProblem = user.ProblemStates.SingleOrDefault(ps => ps.Id == problemStateId);

        }
    }

    public class GameState
    {

        public Guid GameStateId { get; set; }
        public bool GameCompleted { get; set; }

        public Guid LastUnlockedProblemId { get; set; }
        public ProblemState LastUnlockedProblem { get; set; }

        public Person Person { get; set; }
    }

    public class Person
    {
        public Person()
        {
            this.ProblemStates = new List<ProblemState>();
        }
        public Guid Id { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public GameState GameState { get; set; }

        public List<ProblemState> ProblemStates { get; set; }
    }

    public class ProblemState
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public string LevelName { get; set; }
        public string ProblemName { get; set; }
        public string ProblemBlocksXml { get; set; }
        public int ProblemScore { get; set; }
        public bool ProblemLocked { get; set; }

        public bool ProblemCompleted { get; set; }

    }
}
