using UnityEngine;

namespace Scripts.Behaviours
{
    public class Mover : MonoBehaviour
    {

        public float Speed;
        // Use this for initialization
        void Start ()
        {
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * this.Speed * Time.deltaTime;
        }
	
	
    }
}
