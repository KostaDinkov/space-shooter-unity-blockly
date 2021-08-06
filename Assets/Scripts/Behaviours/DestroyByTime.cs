using UnityEngine;

namespace Scripts.Behaviours
{
    public class DestroyByTime : MonoBehaviour {

        // Use this for initialization
        public float lifetime;
        void Start ()
        {
            Destroy(this.gameObject,this.lifetime);
        }
	
	
    }
}
