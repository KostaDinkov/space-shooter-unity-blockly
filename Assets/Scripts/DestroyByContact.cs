using Easy.MessageHub;
using UnityEngine;
using Game.Systems;

public class DestroyByContact : MonoBehaviour
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
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (this.CompareTag("Player"))
        {
            hub.Publish(new Game.Systems.GameEvents.PlayerDied());
            this.GetComponent<Playercontroller>().Die();
            return;
        }
        
        Destroy(this.gameObject);
    }
}