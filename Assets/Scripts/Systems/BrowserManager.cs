using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Easy.MessageHub;
using System.Runtime.InteropServices;

namespace Game.Systems
{
  public class BrowserManager 
  {
    [DllImport("__Internal")]
    private static extern void TestBrowser();
    private MessageHub hub;

    public BrowserManager()
    {
        this.hub = MessageHub.Instance;
        Debug.Log("calling browser with test function");
        var token = hub.Subscribe<Game.Systems.GameEvents.SceneLoaded>(sl=> TestBrowser());
    }
  }
}
