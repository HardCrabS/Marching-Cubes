using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGun : Gun
{
    public GameObject rocket;

    protected override void Initialize()
    {
        base.Initialize();

        onAmmoUpdated += (currAmmo, maxAmmo) =>
        {
            if (currAmmo == maxAmmo)
            {
                rocket.SetActive(true);
            }
        };
    }

    protected override void Shoot()
    {
        if (IsShootAllowed())
        {
            rocket.SetActive(false);
        }

        base.Shoot();
    }
}
