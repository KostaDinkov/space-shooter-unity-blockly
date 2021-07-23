using System.Collections.Generic;

using UnityEngine;


namespace Game.Objectives
{
    /// <summary>
    /// A container class that holds all objectives for a given challange
    /// </summary>
    public class Objectives : MonoBehaviour
    {

        public List<Objective> ObjectiveList = new List<Objective>()
        {
            new Objective(){ Description = "This is the long description for objective one", TargetValue = 1},
            new Objective(){Description = " This is the long description for objective one", TargetValue = 2},
        };


    }
}