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
                    ComplexProp complexProp = child.GetComponent<ComplexProp>();
                    if (!complexProp)
                        complexProp = child.gameObject.AddComponent<ComplexProp>();
                    Vector3 childRayStart = rayStart + child.localPosition;
                    if (!Physics.Raycast(childRayStart, Vector3.down, out hit, Mathf.Infinity))
                        continue;
                    SpawnComplexProp(complexProp, child.gameObject, terrainTransform, hit);
                }
            }
            else
            {
                SpawnProp(placementProps, terrainTransform, hit);
            }
        }
    }

    public static RaycastHit GetRandomHitAtChunk(Transform chunkTransform, float chunkSize)
    {
        float sampleX = Random.Range(0, chunkSize);
        float sampleY = Random.Range(0, chunkSize);
        Vector3 rayStart = new Vector3(sampleX, 100f, sampleY) + chunkTransform.position;

        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            return new RaycastHit();

        return hit;
    }

    private static void SpawnProp(PlacementProps placementProps, Transform terrainTransform, RaycastHit hit)
    {
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

    private static void SpawnComplexProp(ComplexProp complexProp, GameObject prefab, Transform terrainTransform, RaycastHit hit)
    {
        if (Vector3.Angle(hit.normal, Vector3.up) > 45f)
            return;

        GameObject instantiatedPrefab = Instantiate(prefab, terrainTransform);
        instantiatedPrefab.transform.position = hit.point;

        instantiatedPrefab.transform.rotation = Quaternion.Lerp(terrainTransform.rotation, terrainTransform.rotation *
            Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), complexProp.rotateTowardsNormal);
        if (complexProp.rotationRange.y > 0)
            instantiatedPrefab.transform.Rotate(instantiatedPrefab.transform.up, Random.Range(complexProp.rotationRange.x, complexProp.rotationRange.y), Space.Self);
        else
            instantiatedPrefab.transform.rotation = prefab.transform.rotation;

        Vector3 scale = instantiatedPrefab.transform.localScale;
        scale.x *= 1f + Random.Range(-complexProp.scalePercent, complexProp.scalePercent);
        scale.y *= 1f + Random.Range(-complexProp.scalePercent, complexProp.scalePercent);
        scale.z *= 1f + Random.Range(-complexProp.scalePercent, complexProp.scalePercent);
        instantiatedPrefab.transform.localScale = scale;
    }
}
