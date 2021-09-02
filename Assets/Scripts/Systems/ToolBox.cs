using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Systems
{
    [CreateAssetMenu]
    public class ToolBox : ScriptableObject
    {
        public string kind = "categoryToolbox";
        public List<CategoryNode> contents = new List<CategoryNode>();

        

        [Serializable]
        public class BlockNode:ISerializationCallbackReceiver
        {
            [HideInInspector] public string kind;
            public string type;
            public void OnBeforeSerialize()
            {
                
            }

            public void OnAfterDeserialize()
            {
                this.kind = "block";
            }
        }

        [Serializable]
        public class CategoryNode:ISerializationCallbackReceiver
        {
            [HideInInspector] public string kind;
            public string name;
            public string categoryStyle;
            public List<BlockNode> contents;


            public void OnBeforeSerialize()
            {
                this.kind = "category";
            }

            public void OnAfterDeserialize()
            {
                this.kind = "category";
            }
        }
    }

   

    
}