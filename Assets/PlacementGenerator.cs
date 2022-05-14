using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    public static void Generate(PlacementProps placementProps, Transform terrainTransform)
    {
        for (int i = 0; i < placementProps.density; i++)
        {
            float sampleX = Random.Range(placementProps.xRange.x, placementProps.xRange.y);
            float sampleY = Random.Range(placementProps.zRange.x, placementProps.zRange.y);
            Vector3 rayStart = new Vector3(sampleX, placementProps.maxHeight, sampleY) + terrainTransform.position;

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;

            if (hit.point.y < placementProps.minHeight)
                continue;

            GameObject instantiatedPrefab = Instantiate(placementProps.prefab, terrainTransform);
            instantiatedPrefab.transform.position = hit.point;
            instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(placementProps.rotationRange.x, placementProps.rotationRange.y), Space.Self);
            instantiatedPrefab.transform.rotation = Quaternion.Lerp(terrainTransform.rotation, terrainTransform.rotation * 
                Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), placementProps.rotateTowardsNormal);
            instantiatedPrefab.transform.localScale = new Vector3(
                Random.Range(placementProps.minScale.x, placementProps.maxScale.x),
                Random.Range(placementProps.minScale.y, placementProps.maxScale.y),
                Random.Range(placementProps.minScale.z, placementProps.maxScale.z)
            );
        }
    }
}
