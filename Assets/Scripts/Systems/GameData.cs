
using System;
using UnityEngine;


namespace Game.Systems
{
    public class GameData
    {
        private static readonly GameData instance = new GameData();
        

        //Todo Change this to 0 after testing
        public int CurrentChallengeNumber = 1;
        public GameObject CurrentChallenge;
        public int ChallengeCount = 0;
        public int GridSize = 2;

        public static GameData Instance =>instance;

        private GameData() { }

        static GameData() { }

    }
}