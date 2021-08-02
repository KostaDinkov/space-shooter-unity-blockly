using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestinationReachedBehaviour : MonoBehaviour
{
    private GameEventManager eventManager;
    
    private void Start()
    {
        //this.hub = MessageHub.Instance;
        eventManager = GameEventManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            eventManager.Publish(new GameEvent
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
            eventManager.Publish(new GameEvent
            {
                EventType = GameEventType.TargetReached,
                EventValue = -1
            });
        }
    }
}