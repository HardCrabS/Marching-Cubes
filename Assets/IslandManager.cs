using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public Transform chunksHolder;
    public bool isRandomAtStart = true;
    public float mapScaleFactor = 0.6f;

    void Start()
    {
        if (isRandomAtStart)
        {
            MapGenerator mapGen = FindObjectOfType<MapGenerator>();
            mapGen.seed = Random.Range(0, int.MaxValue);
        }
        MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();
        if (meshGen)
        {
            meshGen.InitChunks();
        }
        chunksHolder.localScale *= mapScaleFactor;
    }
}
