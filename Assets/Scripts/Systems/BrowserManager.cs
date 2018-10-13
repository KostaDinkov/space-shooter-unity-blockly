using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;

namespace Game.Systems
{
    public class BrowserManager
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void TestBrowser();

        [DllImport("__Internal")]
        private static extern void ChallangeCompleted();

        [DllImport("__Internal")]
        private static extern void PlayerDied();
#else
        private void ChallangeCompleted(int value){
            Debug.Log("Event Sent To Browser :Challange Completed");    
        }
#endif


        private GameEventManager eventManager;
        private GameEvent challangeCompletedEvent;
        private GameEvent playerDiedEvent;

        public BrowserManager()
        {
            this.eventManager = GameEventManager.Instance;
            

            eventManager.Subscribe( GameEventType.ChallangeCompleted, ChallangeCompleted);
            //eventManager.Subscribe(sl => PlayerDied());

        }
    }
}