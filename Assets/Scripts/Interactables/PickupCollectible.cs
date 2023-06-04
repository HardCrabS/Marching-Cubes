using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PickupCollectible : MonoBehaviour, IInteractable
{
    public CollectibleData collectibleData;

    public float movementSpeed = 5f;
    public float arrivalDistance = 1f;
    public float focalLength = 170;

    float initFocalLenth;
    DepthOfField depthOfField;

    public void Interact()
    {
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.UI);

        Transform playerCamera = Player.Instance.GetComponentInChildren<Camera>().transform;
        playerCamera.GetComponent<FirstPersonLook>().enabled = false;

        var ppvolume = playerCamera.GetComponent<PostProcessVolume>();
        if (ppvolume.profile.TryGetSettings(out depthOfField))
        {
            initFocalLenth = depthOfField.focalLength;
            depthOfField.focalLength.value = focalLength;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventsDispatcher.Instance.onEscape += Escape;

        StartCoroutine(MoveToCameraCoroutine(playerCamera));
    }

    private IEnumerator MoveToCameraCoroutine(Transform playerCamera)
    {
        // Calculate the initial position and direction of the object
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = playerCamera.position;
        Vector3 direction = (targetPosition - startPosition).normalized;

        // Move towards the camera until the arrival distance is reached
        while (Vector3.Distance(transform.position, targetPosition) > arrivalDistance)
        {
            // Calculate the next position based on the movement speed and direction
            Vector3 nextPosition = transform.position + direction * movementSpeed * Time.deltaTime;

            // Move the object towards the next position
            transform.position = nextPosition;

            yield return null;
        }

        EventsDispatcher.Instance.onCollectibleFound?.Invoke(collectibleData);
        gameObject.AddComponent<ObjectRotation>();
    }

    void Escape()
    {
        StopAllCoroutines();
        EventsDispatcher.Instance.onMouseControlChanged?.Invoke(MouseControl.Shooting);
        Transform playerCamera = Player.Instance.GetComponentInChildren<Camera>().transform;
        playerCamera.GetComponent<FirstPersonLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        depthOfField.focalLength.value = initFocalLenth;

        EventsDispatcher.Instance.onEscape -= Escape;

        Destroy(gameObject);
    }
}
