using UnityEngine;

public class DestroyOnCollisionNoEvent : MonoBehaviour
{
    public GameObject explosion;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            return;
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
        
    }
}