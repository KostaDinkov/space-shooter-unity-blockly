using System.Runtime.InteropServices;
using Scripts.GameEvents;

namespace Scripts.Systems
{
    public class BrowserManager
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void TestBrowser();

        [DllImport("__Internal")]
        private static extern void ChallangeCompleted(int value);

        [DllImport("__Internal")]
        private static extern void PlayerDied();
#else
        private void ProblemCompleted(int value){
            //Debug.Log("Event Sent To Browser :Challange Completed");    
        }
#endif


        private GameEventManager eventManager;
        private GameEvent challangeCompletedEvent;
        private GameEvent playerDiedEvent;

        public BrowserManager()
        {
            this.eventManager = GameEventManager.Instance;
            

            //eventManager.Subscribe( GameEventType.ProblemCompleted, ProblemCompleted);
            //eventManager.Subscribe(sl => PlayerDied());

        }
    }
}