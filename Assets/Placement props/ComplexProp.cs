using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data component
public class ComplexProp : MonoBehaviour
{
    [Header("Prefab variations")]
    [Range(0f, 1f)] public float rotateTowardsNormal;
    [Range(0f, 1f)] public float scalePercent = 0.2f;
    public Vector2 rotationRange;
}
