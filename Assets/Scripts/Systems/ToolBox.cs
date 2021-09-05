using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Scripts.Systems
{
    [CreateAssetMenu]
    public class ToolBox : ScriptableObject
    {

        
        public string kind = "categoryToolbox";
        [LabelText("Categories")]
        public List<CategoryNode> contents = new List<CategoryNode>();

        [Serializable]
        public class BlockNode:ISerializationCallbackReceiver
        {
            [HideInInspector] 
            public string kind;
            [HideLabel]
            [ValueDropdown("allBlocks")]
            public string type;
            public void OnBeforeSerialize()
            {
                
            }
            public void OnAfterDeserialize()
            {
                this.kind = "block";
            }
            private string[] allBlocks = new string[]
            {
                "move_forward",
                "rotate_left",
                "rotate_right",
                "fire_weapon",
                "scan_ahead",
                "pickup_object",
                "print",
                "on_start"
            };
        }

        [Serializable]
        
        [InlineProperty(LabelWidth = 50)]
        public class CategoryNode:ISerializationCallbackReceiver
        {
            
            [HideInInspector] 
            public string kind;
            [HorizontalGroup]
            public string name;
            [HorizontalGroup()]
            [LabelText("Style")]
            public string categoryStyle;
            [LabelText("Blocks")]
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