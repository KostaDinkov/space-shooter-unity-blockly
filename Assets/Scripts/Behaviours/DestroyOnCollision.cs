using Assets.Scripts.GameEvents;
using Game.GameEvents;
using Game.SpaceObject;
using UnityEngine;

[RequireComponent(typeof (ISpaceObject))]
public class DestroyOnCollision : MonoBehaviour
{
    public GameObject explosion;
    [Tooltip("The event type to fire when the object is destroyed.")]
    public GameEventType EventType;
    private GameEventManager eventManager = GameEventManager.Instance;

    private void OnTriggerEnter(Collider other)
    {
        

        if (!this.GetComponent<ISpaceObject>().IsDestroyable)
        {
            return;
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        gameObject.SetActive(false);
        eventManager.Publish(new GameEvent(){EventType = EventType, EventValue = 1});
    }
}