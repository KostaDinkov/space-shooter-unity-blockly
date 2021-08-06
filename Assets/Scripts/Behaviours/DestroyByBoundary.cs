using UnityEngine;

namespace Scripts.Behaviours
{
    public class DestroyByBoundary : MonoBehaviour {

        void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
