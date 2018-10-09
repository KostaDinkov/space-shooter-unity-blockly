
using Assets.Scripts.GameEvents;
using Easy.MessageHub;
using Game.GameEvents;
using Game.Systems.GameEvents;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestinationReachedBehaviour : MonoBehaviour
{
    public GameEvent Event;
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
            eventManager.Publish(this.Event);
            
        }
    }
}
