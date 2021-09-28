using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Scripts.Objectives
{
    /// <summary>
    /// A container class that holds all objectives for a given problem
    /// </summary>
    public class Objectives : MonoBehaviour
    {
        [ShowInInspector] [TextArea] [Required]
        public string ProblemDescription;

        public List<Objective> ObjectiveList = new List<Objective>()
        {
            new Objective() {Description = "This is the long description for objective one", TargetValue = 1},
            new Objective() {Description = " This is the long description for objective one", TargetValue = 2},
        };
    }
}