using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    public static void Generate(PlacementProps placementProps, Transform terrainTransform, float chunkSize)
    {
        for (int i = 0; i < placementProps.density; i++)
        {
            float sampleX = Random.Range(0, chunkSize);
            float sampleY = Random.Range(0, chunkSize);
            Vector3 rayStart = new Vector3(sampleX, placementProps.maxHeight, sampleY) + terrainTransform.position;

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;

            if (hit.point.y < placementProps.minHeight)
                continue;

            if (placementProps.isComplexProp)
            {
                // raycast for each child to find a proper position
                foreach (Transform child in placementProps.prefab.transform)
                {
                    rayStart += child.localPosition;
                    if (!Physics.Raycast(rayStart, Vector3.down, out hit, Mathf.Infinity))
                        continue;
                    SpawnProp(child.gameObject, placementProps, terrainTransform, hit);
                }
            }
            else
            {
                SpawnProp(placementProps.prefab, placementProps, terrainTransform, hit);
            }
        }
    }

    private static void SpawnProp(GameObject prefab, PlacementProps placementProps, Transform terrainTransform, RaycastHit hit)
    {
        GameObject instantiatedPrefab = Instantiate(prefab, terrainTransform);
        instantiatedPrefab.transform.position = hit.point;
        instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(placementProps.rotationRange.x, placementProps.rotationRange.y), Space.Self);
        instantiatedPrefab.transform.rotation = Quaternion.Lerp(terrainTransform.rotation, terrainTransform.rotation *
            Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), placementProps.rotateTowardsNormal);

        if (placementProps.isComplexProp)
        {
            Vector3 scale = instantiatedPrefab.transform.localScale;
            scale.x *= 1f + Random.Range(-placementProps.scalePercent, placementProps.scalePercent);
            scale.y *= 1f + Random.Range(-placementProps.scalePercent, placementProps.scalePercent);
            scale.z *= 1f + Random.Range(-placementProps.scalePercent, placementProps.scalePercent);
            instantiatedPrefab.transform.localScale = scale;
        }
        else
        {
            instantiatedPrefab.transform.localScale = new Vector3(
                Random.Range(placementProps.minScale.x, placementProps.maxScale.x),
                Random.Range(placementProps.minScale.y, placementProps.maxScale.y),
                Random.Range(placementProps.minScale.z, placementProps.maxScale.z)
            );
        }
    }
}
