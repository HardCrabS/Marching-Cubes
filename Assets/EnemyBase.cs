using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float spawnRadius = 5f;

    public void SpawnEnemy(Enemy enemyPrefab)
    {
        float randOffsetX = Random.Range(-spawnRadius, spawnRadius);
        float randOffsetZ = Random.Range(-spawnRadius, spawnRadius);
        Vector3 rayStart = transform.position + new Vector3(randOffsetX, 0, randOffsetZ) + Vector3.up * 100;

        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            return;

        Instantiate(enemyPrefab.gameObject, hit.point, enemyPrefab.transform.rotation, transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
