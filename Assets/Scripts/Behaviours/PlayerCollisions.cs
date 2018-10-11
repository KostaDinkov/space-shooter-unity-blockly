using Assets.Scripts.GameEvents;
using Game.GameEvents;
using UnityEngine;
using Game.Systems;

public class PlayerCollisions : MonoBehaviour
{
    public GameObject explosion;
    private GameEventManager eventManager;


    void Start()
    {
        this.eventManager = GameEventManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Target"))
        {
            
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        //TODO publish player died event 
        this.eventManager.Publish(new GameEvent {EventType = GameEventType.PlayerDied});
        this.GetComponent<Playercontroller>().Die();
    }
}