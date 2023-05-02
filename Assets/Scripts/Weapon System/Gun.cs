using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform shotPoint;

    public System.Action onPickUp;

    float nextShotTime = 0f;
    bool isReloading = false;
    int currAmmoInMag;

    // Start is called before the first frame update
    void Start()
    {
        currAmmoInMag = weaponData.ammoMax;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (!isReloading && currAmmoInMag == 0)
            Reload();
    }

    public void OnTriggerHold()
    {
        Shoot();
    }

    void Shoot()
    {
        if (currAmmoInMag > 0 && Time.time >= nextShotTime)
        {
            Projectile prj = Instantiate(weaponData.projectile, shotPoint.position, shotPoint.rotation);
            prj.SetSpeed(weaponData.speed);
            currAmmoInMag--;
            nextShotTime = Time.time + weaponData.msBetweenShots / 1000f;

            EventsDispatcher.Instance.onShoot?.Invoke();
        }
    }

    public void Reload()
    {
        if (!isReloading && currAmmoInMag != weaponData.ammoMax)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1 / weaponData.reloadTime;
        float percent = 0;

        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30.0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        currAmmoInMag = weaponData.ammoMax;
    }
}
