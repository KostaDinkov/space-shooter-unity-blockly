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
    //[CustomPropertyDrawer(typeof(CategoryNode))]
    public class ToolBoxInspectors:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect nameRect = new Rect(position.x, position.y, position.width, position.height);

            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
