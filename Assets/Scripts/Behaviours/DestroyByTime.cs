using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	// Use this for initialization
    public float lifetime;
	void Start ()
	{
	    Destroy(gameObject,lifetime);
	}
	
	
}
