using UnityEngine;

namespace Scripts.Behaviours
{
    public class BGScroller : MonoBehaviour
    {

        public float ScrollSpeed;

        public float tileSizeZ;

        private Vector3 startPosition;
        // Use this for initialization
        void Start ()
        {
            this.startPosition = this.transform.position;
        }
	
        // Update is called once per frame
        void Update ()
        {
            float newPosition = Mathf.Repeat(Time.time * this.ScrollSpeed, this.tileSizeZ );
            this.transform.position = this.startPosition + Vector3.forward * newPosition;
        }
    }
}
