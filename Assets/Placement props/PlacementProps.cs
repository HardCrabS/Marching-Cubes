using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Placement props")]
public class PlacementProps : ScriptableObject
{
    public GameObject prefab;

    public int density;

    [Header("Complex prop")]
    [Tooltip("Spawn every child of the prop individually")]
    public bool isComplexProp = false;
    [Range(0f, 1f)] public float scalePercent = 0.2f;

    [Space]

    public float minHeight;
    public float maxHeight;
    public Vector2 xRange;
    public Vector2 zRange;

    [Header("Prefab variations")]
    [Range(0, 1)] public float rotateTowardsNormal;
    public Vector2 rotationRange;
    public Vector3 minScale;
    public Vector3 maxScale;
}
