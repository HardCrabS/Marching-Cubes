using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform shotPoint;

    public System.Action onPickUp;
    public System.Action onGunShoot;

    float nextShotTime = 0f;
    bool isReloading = false;
    int currAmmoInMag;

    SoundPitcher soundPitcher;

    // Start is called before the first frame update
    void Start()
    {
        soundPitcher = gameObject.GetComponent<SoundPitcher>();
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
        if (!isReloading && currAmmoInMag > 0 && Time.time >= nextShotTime)
        {
            Projectile prj = Instantiate(weaponData.projectile, shotPoint.position, shotPoint.rotation);
            prj.SetSpeed(weaponData.speed);
            prj.SetImpactFX(weaponData.impactFX);
            currAmmoInMag--;
            nextShotTime = Time.time + weaponData.msBetweenShots / 1000f;

            EventsDispatcher.Instance.onShoot?.Invoke();
            onGunShoot?.Invoke();

            soundPitcher.PlaySound(weaponData.shotSFX);
        }
    }

    public void Reload()
    {
        if (!isReloading && currAmmoInMag != weaponData.ammoMax)
        {
            StartCoroutine(AnimateReload());
            soundPitcher.PlaySound(weaponData.reloadSFX);
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
