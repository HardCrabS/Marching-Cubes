using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGen = (MeshGenerator)target;

        if(DrawDefaultInspector())
            meshGen.GenerateMesh();

        if (GUILayout.Button("Generate"))
            meshGen.GenerateMesh();
    }
}
