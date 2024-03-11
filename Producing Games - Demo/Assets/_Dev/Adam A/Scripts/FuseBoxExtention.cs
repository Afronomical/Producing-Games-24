using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//[CustomEditor(typeof(FuseBox))]
//public class FuseBoxExtention : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        // Copy from object to serialized data
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(serializedObject.FindProperty("cursor"));  // Show all of the FuseBox properties
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("fuses"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("fuseSlots"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("materials"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("complete"));

//        if (GUILayout.Button("Reset default fuse positions"))
//        {
//            FuseBox box = GameObject.Find("Fuse Box").GetComponent<FuseBox>();  // Find the Fuse box
//            foreach (GameObject w in box.fuses)  // For each Fuse in the box
//            {
//                FuseBox fuse = w.GetComponent<FuseBox>();
//            }
//        }


//        // Copy from serialized data to object
//        serializedObject.ApplyModifiedProperties();
//    }
//}
