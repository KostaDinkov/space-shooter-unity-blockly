using System;
using UnityEngine;


namespace Game.Systems
{
    public class GameData: MonoBehaviour
    {
        private static  GameData instance;
        public static GameData Instance =>instance;
        public Objectives.Objectives Objectives;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
        public int SceneCount = 0;
        public int GridSize = 2;
    }
}