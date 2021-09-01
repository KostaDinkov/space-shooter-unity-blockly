
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Systems
{
    [CreateAssetMenu]
    public class ToolBox:ScriptableObject
    {
        
        public ToolBoxType ToolBoxType = new ToolBoxType(){contents = new List<CategoryNode>(), kind = "categoryToolbox" };

        
    }

    [System.Serializable]
    public class BlockNode
    {
        public string kind = "block";
        public string type;

    }

    [System.Serializable]
    public class CategoryNode
    {
        public string kind = "category";
        public string name;
        public string categoryStyle;
        public List<BlockNode> contents;
    }

    [System.Serializable]
    public class ToolBoxType
    {
        public string kind;
        public List<CategoryNode> contents;
    }
}
