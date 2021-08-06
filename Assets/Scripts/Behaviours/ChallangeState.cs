using Scripts.GameEvents;
using UnityEngine;

namespace Scripts.Behaviours
{
    public class ChallangeState : MonoBehaviour
    {
        private GameEventManager eventManager = GameEventManager.Instance;

        [Tooltip("If the challange is complete")]
        public bool IsComplete;

        void Start()
        {
            this.eventManager.Subscribe(GameEventType.ProblemCompleted, (x) => this.IsComplete = true);
        }

    }
}
