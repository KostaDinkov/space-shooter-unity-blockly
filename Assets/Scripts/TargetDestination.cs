using UnityEngine;
using Easy.MessageHub;

public class TargetDestination : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
          var hub = MessageHub.Instance;
          hub.Publish(new ObjectiveCompleted());
            
        }
    }
	// Update is called once per frame
	
}
public class ObjectiveCompleted{}
