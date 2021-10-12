using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.SpaceObject
{
    [CreateAssetMenu]
    public class SpaceObjectModelsMap : ScriptableObject
    {
        public List<SpaceObjectModel> ModelMap = new List<SpaceObjectModel>();

        [Serializable]
        
        public class SpaceObjectModel : ISerializationCallbackReceiver
        {
            [HorizontalGroup("Group1", LabelWidth = 120)] 
            [HideLabel]
            public SpaceObjectType SpaceObjectType;
            [HorizontalGroup("Group1")]
            [LabelText("Models Prefab")] 
            public GameObject Model;
            public void OnBeforeSerialize()
            {

            }

            public void OnAfterDeserialize()
            {

            }
        }
    }

    


}