
using UnityEngine;

namespace Assets.Scripts.Objectives
{
    abstract class Objective<T>:MonoBehaviour
    {
        public string Description;
        public T CurrentValue;
        public T TargetValue;
        public abstract bool IsComplete();
    }
}
