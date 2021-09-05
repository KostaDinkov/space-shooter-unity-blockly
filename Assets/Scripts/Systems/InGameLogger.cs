using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
