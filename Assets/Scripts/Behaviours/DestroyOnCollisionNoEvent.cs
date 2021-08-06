using UnityEngine;

namespace Scripts.Behaviours
{
    public class DestroyOnCollisionNoEvent : MonoBehaviour
    {
        public GameObject explosion;
   
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Boundary"))
            {
                return;
            }
            if (this.explosion != null)
            {
                Instantiate(this.explosion, this.transform.position, this.transform.rotation);
            }

            Destroy(this.gameObject);
        
        }
    }
}