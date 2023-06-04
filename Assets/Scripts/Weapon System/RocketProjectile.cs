using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : Projectile
{
    public float explosionRadius = 6f;
    public int terrainEditingRadius = 3;

    protected override void OnHitObject(Collider c, Vector3 hitPoint)
    {
        EditTerrain(c.transform, hitPoint);

        base.OnHitObject(c, hitPoint);
    }

    protected override void HandleDamage(Collider c)
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, collisionMask, QueryTriggerInteraction.Collide);
        foreach (var col in colliders)
        {
            HealthSystem hs = col.GetComponent<HealthSystem>();
            if (hs)
            {
                hs.TakeDamage(damage);
            }
        }
    }

    void EditTerrain(Transform hitTransform, Vector3 hitPoint)
    {
        if (hitTransform.GetComponent<Chunk>())
            ProcessChunk(hitTransform.GetComponent<Chunk>(), hitPoint, 10);
    }

    void ProcessChunk(Chunk chunk, Vector3 hitPos, float isolevelDiff)
    {
        Player.Instance.GetComponent<FirstPersonMovement>().NotifyTerrainChange(hitPos, terrainEditingRadius);
        EndlessTerrain.Instance.EditChunkPoints(chunk, hitPos, isolevelDiff, terrainEditingRadius);
    }
}
