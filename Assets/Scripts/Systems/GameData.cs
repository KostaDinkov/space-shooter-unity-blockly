using System.Collections.Generic;
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
        public int LastUnlockedProblem { get; set; }
        public int ProblemsCount { get;  set; }
        public int CurrentProblem { get; set; }
        public string CurrentProblemName { get; set; }
        public string CurrentLevelName { get; set; }
        public SortedDictionary<string,List<ProblemState>> UserProblemStates { get; set; }


        private GameData()
        {
            this.gameEventManager = GameEventManager.Instance;
            this.ProblemsCount = SceneManager.sceneCountInBuildSettings;
            //TODO the game will probably start with a different scene, so the first problem will be with a different index
            this.LastUnlockedProblem = 2;
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

       

        
    }
}