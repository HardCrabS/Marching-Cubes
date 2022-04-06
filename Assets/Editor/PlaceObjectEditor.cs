using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceObject))]
public class PlaceObjectEditor : Editor
{
    MeshGenerator meshGen;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Activate"))
        {
            SceneView.duringSceneGui += this.OnSceneGUI;
        }
        if(GUILayout.Button("Stop"))
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
    }
    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // We use hotControl to lock focus onto the editor (to prevent deselection)
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlID;
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;
                Debug.Log("Mouse Down!");
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.GetComponent<Chunk>())
                        ProcessChunk(hit.transform.GetComponent<Chunk>(), hit.point);
                }

                Event.current.Use();
                break;

            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                Event.current.Use();
                break;

        }
    }

    void ProcessChunk(Chunk chunk, Vector3 hitPos)
    {
        if (meshGen == null)
            meshGen = FindObjectOfType<MeshGenerator>();
        PlaceObject po = (PlaceObject)target;
        int stencilSize = po.stencilSize;
        Vector3 chunkPos = (Vector3)chunk.coord * (meshGen.pointsPerAxis - 1) * meshGen.pointsOffset;
        Vector3Int pointPos = Vector3Int.RoundToInt((hitPos - chunkPos) / meshGen.pointsOffset);
        
        for (int x = -stencilSize; x <= stencilSize; x++)
        {
            for (int y = -stencilSize; y <= stencilSize; y++)
            {
                for (int z = -stencilSize; z <= stencilSize; z++)
                {
                    chunk.UpdatePointIsolevel(pointPos + new Vector3Int(x, y, z), -10);

                }
            }
        }

        meshGen.UpdateChunkMesh(chunk);
    }
}
