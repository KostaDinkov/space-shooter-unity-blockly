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
                "on_start",
                // --- loops ---
                "controls_repeat",
                "controls_repeat_ext",
                "controls_whileUntil",
                "controls_for",
                "controls_forEach",
                "controls_flow_statements",
                // --- logic ---
                "controls_if",
                "logic_compare",
                "logic_operation",
                "logic_negate",
                "logic_boolean",
                "logic_null",
                "logic_ternary",
                // --- lists ---
                "lists_create_empty",
                "lists_create_with",
                "lists_repeat",
                "lists_length",
                "lists_isEmpty",
                "lists_indexOf",
                "lists_getIndex",
                "lists_setIndex",
                "lists_getSublist",
                // --- text ---
                "text",
                "text_join",
                "text_append",
                "text_length",
                "text_isEmpty",
                "text_indexOf",
                "text_charAt",
                "text_getSubstring",
                "text_changeCase",
                "text_trim",
                // --- math ---
                "math_number",
                "math_arithmetic",
                "math_single",
                "math_constant",
                "math_number_property",
                "math_change",
                "math_on_list",
                "math_modulo",
                "math_constrain",
                "math_random_int",
                "math_random_float",
                // --- variables ---
                "variables_get",
                "variables_set",
                "variables_get_string",
                "variables_set_string",
                // --- procedures ---
                "procedures_defreturn",
                "procedures_callreturn",
                "procedures_callnoreturn",
                "procedures_ifreturn"



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