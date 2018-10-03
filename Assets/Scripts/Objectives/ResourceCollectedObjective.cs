using System.Threading;

namespace Assets.Scripts.Objectives
{
    class ResourceCollectedObjective : Objective<int>
    {
        public override bool IsComplete()
        {
            return this.CurrentValue >= this.TargetValue;
        }
    }
}