using Scripts.GameEvents;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Behaviours
{
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
            Debug.Log($"Player colission: {other.name}");
            if (other.CompareTag("Boundary") || other.CompareTag("Landing"))
            {
                return;
            }
            if (this.explosion != null)
            {
                Instantiate(this.explosion, this.transform.position, Quaternion.Euler(0,0,0));
            }


            this.eventManager.Publish(new GameEvent {EventType = GameEventType.PlayerDied});
            this.GetComponent<Playercontroller>().Die();
        }
    }
}