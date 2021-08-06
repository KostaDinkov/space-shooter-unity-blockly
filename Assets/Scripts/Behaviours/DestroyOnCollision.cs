using Scripts.GameEvents;
using Scripts.SpaceObject;
using UnityEngine;

namespace Scripts.Behaviours
{
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
            if (this.explosion != null)
            {
                Instantiate(this.explosion, this.transform.position, this.transform.rotation);
            }

            this.gameObject.SetActive(false);
            this.eventManager.Publish(new GameEvent(){EventType = this.EventType, EventValue = 1});
        }
    }
}