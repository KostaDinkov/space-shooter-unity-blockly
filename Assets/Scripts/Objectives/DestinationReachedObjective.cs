using System;
using UnityEngine;

namespace Assets.Scripts.Objectives
{
    class DestinationReachedObjective : Objective<bool>
    {
        public override bool IsComplete()
        {
            return this.CurrentValue == TargetValue;
        }
    }
}