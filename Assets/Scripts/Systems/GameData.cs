using Scripts.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems
{
    public class GameData : MonoBehaviour
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
            this.LastUnlockedProblem = 0;
            this.CurrentProblem = 0;
            
        }

       

        
    }
}