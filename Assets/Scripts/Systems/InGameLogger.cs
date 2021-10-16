using Mono.Web;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class InGameLogger
    {
        public void Log(string msg)
        {
            
            Debug.Log($"<color=#00b2ff>[INFO]</color> - {msg}");
        }
    }
}