using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSelectTerminal : MonoBehaviour, IInteractable
{
    public GameObject islandsCanvas;
    public GameObject uiCamera;

    public void Interact()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.UI);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        islandsCanvas.SetActive(true);
        uiCamera.SetActive(true);

        EventsDispatcher.Instance.onEscape += Exit;
    }

    void Exit()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.Shooting);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        islandsCanvas.SetActive(false);
        uiCamera.SetActive(false);

        EventsDispatcher.Instance.onEscape -= Exit;
    }
}