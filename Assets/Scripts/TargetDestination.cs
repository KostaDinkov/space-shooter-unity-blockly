using UnityEngine;
using Easy.MessageHub;

public class TargetDestination : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      var hub = MessageHub.Instance;
      hub.Publish(new Game.Events.ChallengeCompleted());

    }
  }
}

