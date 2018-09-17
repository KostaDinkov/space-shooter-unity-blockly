using Easy.MessageHub;
using System.Runtime.InteropServices;

namespace Game.Systems
{
    public class BrowserManager
    {
        [DllImport("__Internal")]
        private static extern void TestBrowser();

        [DllImport("__Internal")]
        private static extern void ChallangeCompleted();

        [DllImport("__Internal")]
        private static extern void PlayerDied();

        private MessageHub hub;


        public BrowserManager()
        {
            this.hub = MessageHub.Instance;
            
            hub.Subscribe<Game.Systems.GameEvents.SceneLoaded>(sl => TestBrowser());
            hub.Subscribe<Game.Systems.GameEvents.ChallengeCompleted>(sl => ChallangeCompleted());
            hub.Subscribe<Game.Systems.GameEvents.PlayerDied>(sl => PlayerDied());
        }
    }
}