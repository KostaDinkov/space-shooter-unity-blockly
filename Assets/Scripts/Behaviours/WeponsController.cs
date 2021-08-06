
using UnityEngine;

namespace Scripts.Behaviours
{
    public class WeponsController : MonoBehaviour {

        // Use this for initialization
        private AudioSource audioSource;
        public GameObject shot;
        public Transform shotSpawn;
        public float FireRate;
        public float shotDelay;

        void Start ()
        {
            this.audioSource = this.GetComponent<AudioSource>();
            this.InvokeRepeating("FireWeapon", this.shotDelay, this.FireRate);
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        void FireWeapon()
        {
            Instantiate(this.shot, this.shotSpawn.position, this.shotSpawn.rotation);
            this.audioSource.Play();
        }
    }
}
