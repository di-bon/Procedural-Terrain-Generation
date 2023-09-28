//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[ExecuteInEditMode]
//[CustomEditor(typeof(TerrainGenerator))]
//[CanEditMultipleObjects]
//public class TerrainGeneratorEditor : Editor
//{
//    SerializedObject terrainGenerator;

//    void OnEnable()
//    {
//        terrainGenerator = serializedObject;
//    }

//    public override void OnInspectorGUI()
//    {
//        EditorGUI.BeginChangeCheck();
//        base.OnInspectorGUI();
//        if (EditorGUI.EndChangeCheck())
//        {
//            terrainGenerator.Update();
//            serializedObject.Update();
//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}
