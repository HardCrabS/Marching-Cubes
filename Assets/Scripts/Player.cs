using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Dead params")]
    public float deadYHeadPosition = 1;
    public float deadColliderHeight = 0.5f;
    public float deadKickForce = 5f;

    public static Player Instance;

    public Action<int, int> onAmmoUpdated;

    HealthSystem healthSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Kill();
        }
    }

    public void Initialize()
    {
        GunController gunController = GetComponent<GunController>();

        EventsDispatcher.Instance.onInteract += gunController.EquipGun;
        EventsDispatcher.Instance.onTriggerHold += gunController.OnTriggerHold;
        EventsDispatcher.Instance.onReload += gunController.Reload;
        gunController.onGunSwitched += HandleSwitchGun;

        gunController.Initialize();

        healthSystem = GetComponent<HealthSystem>();
        var hd = healthSystem.GetHealthData();
        EventsDispatcher.Instance.onPlayerHealthUpdated?.Invoke(hd);
    }

    void HandleSwitchGun(Gun gun)
    {
        var ammoInfo = gun.GetAmmoInfo();
        onAmmoUpdated?.Invoke(ammoInfo.Item1, ammoInfo.Item2);
        gun.onAmmoUpdated += onAmmoUpdated;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        var hd = healthSystem.GetHealthData();
        EventsDispatcher.Instance.onPlayerHealthUpdated?.Invoke(hd);
    }

    public override void Kill()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        GetComponent<FirstPersonMovement>().enabled = false;
        GetComponentInChildren<FirstPersonLook>().enabled = false;
        GunController gunController = GetComponent<GunController>();
        EventsDispatcher.Instance.onInteract -= gunController.EquipGun;
        EventsDispatcher.Instance.onTriggerHold -= gunController.OnTriggerHold;
        EventsDispatcher.Instance.onReload -= gunController.Reload;
        gunController.onGunSwitched += HandleSwitchGun;
        gunController.FinalizeCtrl();

        Transform headToLower = GetComponentInChildren<Camera>().transform;
        CapsuleCollider colliderToLower = GetComponentInChildren<CapsuleCollider>();
        headToLower.localPosition = new Vector3(headToLower.localPosition.x, deadYHeadPosition, headToLower.localPosition.z);
        colliderToLower.height = deadColliderHeight;

        rb.freezeRotation = true;
        rb.AddForce(-headToLower.forward * deadKickForce, ForceMode.Impulse);

        EventsDispatcher.Instance.onPlayerDead?.Invoke();
    }

    public void FreezeControls()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.UI);

        Transform playerCamera = GetComponentInChildren<Camera>().transform;
        playerCamera.GetComponent<FirstPersonLook>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
