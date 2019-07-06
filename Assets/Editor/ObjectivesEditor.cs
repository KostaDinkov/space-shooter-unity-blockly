
#if UNITY_EDITOR

using Game.Objectives;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Objectives))]
public class ObjectivesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorList.Show(serializedObject.FindProperty("ObjectiveList"));
        
        if (GUILayout.Button("Add New Objective"))
        {
            ((Objectives)target).ObjectiveList.Add(new Objective());
        }
        serializedObject.ApplyModifiedProperties();
    }
        
    
}

public static class EditorList
{
    public static void Show(SerializedProperty list)
    {
        //EditorGUILayout.PropertyField(list);
        
        var guiStyle = new GUIStyle(){richText = true};
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.LabelField($"<b>Objective {i + 1}</b>", guiStyle);
            
            
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("Description"));
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("ListenEvent"));
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("DefaultValue"));
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("TargetValue"));
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("CurrentValue"));
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Width(Screen.width / 4f)))
            {
                list.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel -= 1;
        }
        
    }
}
#endif