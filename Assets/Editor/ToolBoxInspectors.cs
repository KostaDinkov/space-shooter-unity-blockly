using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scripts.Systems;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Editor
{
    //[CustomPropertyDrawer(typeof(ToolBox.CategoryNode))]
    public class ToolBoxInspectors:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            

            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position,label);
            EditorGUI.EndProperty();
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("name")); 
            EditorGUI.EndProperty();

            
            


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLine = 3;

            SerializedProperty nameProperty = property.FindPropertyRelative("name");
            if (nameProperty.isExpanded)
                totalLine++;

            return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine - 1);
        }
    }
}
