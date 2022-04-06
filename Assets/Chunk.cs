using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk : MonoBehaviour
{
    public Vector3Int coord;

    Bounds bounds;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Point[,,] points;

    public void SetUp(Vector3Int coord, float chunkSize, Material mat)
    {
        this.coord = coord;
        Vector3 pos = (Vector3)coord * chunkSize;
        Vector3 center = pos + Vector3.one * chunkSize * 0.5f;
        transform.position = pos;
        bounds = new Bounds(center, Vector3.one * chunkSize);

        meshFilter = GetComponent<MeshFilter>();
        if(meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = mat;

        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
    }

    public void UpdateTerrainChunk(Vector3 viewerPosition, float maxViewDst)
    {
        float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
        bool visible = viewerDstFromNearestEdge <= maxViewDst;
        SetVisible(visible);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

    public void SetMesh(Mesh mesh, Point[,,] points = null)
    {
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        if (points != null)
            this.points = points;
    }

    public Point[,,] GetPoints()
    {
        return points;
    }

    public void UpdatePointIsolevel(Vector3Int pointPos, float isolevelDt)
    {
        if (pointPos.x >= 0 && pointPos.y >= 0 && pointPos.z >= 0 &&
            pointPos.x < points.GetLength(0) && pointPos.y < points.GetLength(1) && pointPos.z < points.GetLength(2))
        points[pointPos.x, pointPos.y, pointPos.z].isolevel += isolevelDt;
    }
}
