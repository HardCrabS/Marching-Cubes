using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelGun : Gun
{
    [Range(0, 7)]
    public int terrainEditingRange = 3;
    public float shootDistance = 5f;
    public float isolevelDiff = 10;

    Camera cam;
    EndlessTerrain endlessTerrain;
    FirstPersonMovement personMovement;

    protected override void Initialize()
    {
        base.Initialize();

        cam = GetComponentInParent<Camera>();
        endlessTerrain = EndlessTerrain.Instance;
        personMovement = GetComponentInParent<FirstPersonMovement>();
    }

    protected override void Shoot()
    {
        if (IsShootAllowed())
        {
            nextShotTime = Time.time + weaponData.msBetweenShots / 1000f;

            EditTerrain();

            EventsDispatcher.Instance.onShoot?.Invoke();
            onAmmoUpdated?.Invoke(currAmmoInMag, weaponData.ammoMax);
            onGunShoot?.Invoke();
        }
    }

    public override void Reload()
    {

    }

    void EditTerrain()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            if (hit.transform.GetComponent<Chunk>())
                ProcessChunk(hit.transform.GetComponent<Chunk>(), hit.point, isolevelDiff);
        }
    }

    void ProcessChunk(Chunk chunk, Vector3 hitPos, float isolevelDiff)
    {
        personMovement.NotifyTerrainChange(hitPos, terrainEditingRange);
        endlessTerrain.EditChunkPoints(chunk, hitPos, isolevelDiff, terrainEditingRange);
    }
}
