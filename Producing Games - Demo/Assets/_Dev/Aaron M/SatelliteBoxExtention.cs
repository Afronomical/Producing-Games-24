using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;

[CustomEditor(typeof(SatelliteBox))]
public class SatelliteBoxExtention : Editor
{
    public override void OnInspectorGUI()
    {
        // Copy from object to serialized data
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cursor"));  // Show all of the SatelliteBox properties
        EditorGUILayout.PropertyField(serializedObject.FindProperty("wires"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("wireEnds"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("materials"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("images"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("complete"));

        if (GUILayout.Button("Reset default wire positions"))
        {
            SatelliteBox box = GameObject.Find("Satellite Box").GetComponent<SatelliteBox>();  // Find the satellite box
            foreach (GameObject w in box.wires)  // For each wire in the box
            {
                SatelliteWire wire = w.GetComponent<SatelliteWire>();
                wire.pointList.Clear();
                wire.pointList.Add(wire.pointA.position);  // Get the start and end points that the line should use
                wire.pointList.Add(wire.wireEnd.transform.position);
                wire.lineRenderer.positionCount = wire.pointList.Count;
                wire.lineRenderer.SetPositions(wire.pointList.ToArray());  // Move the line to those points
            }
        }


        // Copy from serialized data to object
        serializedObject.ApplyModifiedProperties();
    }
}
#endif