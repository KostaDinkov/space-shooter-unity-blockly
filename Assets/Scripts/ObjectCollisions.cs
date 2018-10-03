using UnityEngine;

public class ObjectCollisions : MonoBehaviour
{
    public GameObject explosion;
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(this.gameObject);
    }
}