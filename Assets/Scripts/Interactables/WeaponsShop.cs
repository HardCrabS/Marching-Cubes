using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsShop : MonoBehaviour, IInteractable
{
    public WeaponShopView shopView;
    public GameObject uiCamera;

    public void Interact()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.UI);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        shopView.gameObject.SetActive(true);
        shopView.Init();
        uiCamera.SetActive(true);

        EventsDispatcher.Instance.onEscape += Exit;
    }

    void Exit()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.Shooting);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        shopView.gameObject.SetActive(false);
        shopView.Fini();
        uiCamera.SetActive(false);

        EventsDispatcher.Instance.onEscape -= Exit;
    }
}
