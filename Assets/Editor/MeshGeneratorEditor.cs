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

        DrawDefaultInspector();

        //if (meshGen.autoUpdate)
        //    meshGen.InitChunks();

        if (GUILayout.Button("Generate"))
            meshGen.InitChunks();
    }
}
