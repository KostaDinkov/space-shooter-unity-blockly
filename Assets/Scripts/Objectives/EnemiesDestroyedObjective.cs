using UnityEngine;
using UnityEngine.Analytics;

namespace Assets.Scripts.Objectives
{
    class EnemiesDestroyedObjective:Objective<int>
    {
        public override bool IsComplete()
        {
            return this.CurrentValue >= this.TargetValue;
        }
    }
}
