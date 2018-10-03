using Easy.MessageHub;
using UnityEngine;
using Game.Systems;

public class PlayerCollisions : MonoBehaviour
{
    public GameObject explosion;
    private MessageHub hub;

    void Start()
    {
        this.hub = MessageHub.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Target"))
        {
            hub.Publish(new Game.Systems.GameEvents.ChallengeCompleted());
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        hub.Publish(new Game.Systems.GameEvents.PlayerDied());
        this.GetComponent<Playercontroller>().Die();
    }
}