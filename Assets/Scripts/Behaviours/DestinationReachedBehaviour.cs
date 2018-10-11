using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestinationReachedBehaviour : MonoBehaviour
{
    
    private GameEventManager eventManager;
    //private MessageHub hub;
    // Start is called before the first frame update
    void Start()
    {
        //this.hub = MessageHub.Instance;
        this.eventManager = GameEventManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //hub.Publish(new DestinationReached());
            this.eventManager.Publish(new GameEvent {EventType=GameEventType.TargetReached});
            
        }
    }
}
