using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems
{
    public class GameData 
    {
        private static GameData instance;
        private GameEventManager gameEventManager;

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

        public int LastUnlockedProblem { get; set; }
        public int ProblemsCount { get;  set; }
        public int CurrentProblem { get; set; }


        private GameData()
        {
            this.gameEventManager = GameEventManager.Instance;
            this.ProblemsCount = SceneManager.sceneCountInBuildSettings;
            //TODO the game will probably start with a different scene, so the first problem will be with a different index
            this.LastUnlockedProblem = 0;
            this.CurrentProblem = 0;
            
        }

       

        
    }
}