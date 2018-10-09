using UnityEngine;

public class ObjectCollisions : MonoBehaviour
{
    public GameObject explosion;
    
    void OnTriggerEnter(Collider other)
    {
        

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(this.gameObject);
    }
}