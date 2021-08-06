using Scripts.GameEvents;
using UnityEngine;

namespace Scripts.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class DestinationReachedBehaviour : MonoBehaviour
    {
        private GameEventManager eventManager;
    
        private void Start()
        {
            //this.hub = MessageHub.Instance;
            this.eventManager = GameEventManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.eventManager.Publish(new GameEvent
                {
                    EventType = GameEventType.TargetReached,
                    EventValue = 1
                });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.eventManager.Publish(new GameEvent
                {
                    EventType = GameEventType.TargetReached,
                    EventValue = -1
                });
            }
        }
    }
}