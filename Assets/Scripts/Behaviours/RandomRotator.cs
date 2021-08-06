using UnityEngine;

namespace Scripts.Behaviours
{
    public class RandomRotator : MonoBehaviour
    {

        [SerializeField]private float tumble = 0.1f;
	
        void Start ()
        {
            this.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * this.tumble;
        }
	
    }
}
