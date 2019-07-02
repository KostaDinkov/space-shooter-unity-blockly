using System;
using UnityEngine;
using System.Collections;
using Game.GameEvents;


public class ChallangeState : MonoBehaviour
{
  private GameEventManager eventManager = GameEventManager.Instance;

  [Tooltip("If the challange is complete")]
  public bool IsComplete;

  void Start()
  {
    this.eventManager.Subscribe(GameEventType.ChallangeCompleted, (x) => IsComplete = true);
  }

}
