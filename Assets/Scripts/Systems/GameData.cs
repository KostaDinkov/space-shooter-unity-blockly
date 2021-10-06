using System.Collections.Generic;
using System.Linq;
using AzureSqlDbConnect;
using Models;
using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems
{
    public class GameData 
    {
        private static GameData instance;
        private GameEventManager gameEventManager;
        public DbApi dbApi;
        public Dictionary<string, string> LevelNames { get; private set; }

        public static GameData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameData();
                }

                return instance;
            }
        }


        public const int GridSize = 2;

        public string Username { get; set; }
        public string LastUnlockedProblem { get; set; }
        public int ProblemsCount { get;  set; }
        public int CurrentProblem { get; set; }
        public string CurrentProblemName { get; set; }
        public string CurrentLevelName { get; set; }
        public bool GameComplete { get; set; }
        public SortedDictionary<string,List<ProblemState>> UserProblemStates { get; set; }


        private GameData()
        {
            this.gameEventManager = GameEventManager.Instance;
            this.ProblemsCount = SceneManager.sceneCountInBuildSettings;
            //TODO the game will probably start with a different scene, so the first problem will be with a different index
            
            this.CurrentProblem = 0;
            this.Username = "kosta@kiberlab.net"; 

            this.LevelNames = new Dictionary<string, string>()
            {
                {"l01","Ниво 1 - Контрол"},
                {"l02","Ниво 2 - Цикли"},
                {"l03","Ниво 3 - Условни Конструкции"},
                {"l04","Ниво 4 - Методи"},
                {"l05","Ниво 5 -Масиви"},
            };
        }

        /// <summary>
        /// Gets the next problem from the problemStates dictionary
        /// </summary>
        /// <returns>The name of the next problem (scene name) or null</returns>
        public string GetNextProblemSceneName()
        {
            //generate next problem name based on current problem name
            //for example p01 => p02, p09 => p10
            //then search for that problem in the current level
            var nextProblemNuber = int.Parse(this.CurrentProblemName.Substring(1, 2)) + 1;
            var nextProblemName = "p" + nextProblemNuber.ToString("D2");
            var nextProblemNameExists = this.UserProblemStates[this.CurrentLevelName]
                .FirstOrDefault(p => p.ProblemName == nextProblemName);
            if (nextProblemNameExists != null)
            {
                return this.CurrentLevelName + nextProblemName;
            }

            //if there is no next problem in the current level
            //check if there is a next level and take it's first problem

            var nextLevelNumber = int.Parse(this.CurrentLevelName.Substring(1, 2)) + 1;
            var nextLevelName = "l" + nextLevelNumber.ToString("D2");
            if (this.UserProblemStates.ContainsKey(nextLevelName))
            {
                return nextLevelName + "p01";
            }

            return null;
        }

        public ProblemState FindProblemState(string sceneName)
        {
            return this.UserProblemStates[sceneName.Substring(0, 3)]
                .FirstOrDefault(p => p.ProblemName == sceneName.Substring(3, 3));
        }

        
    }
}