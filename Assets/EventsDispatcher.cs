using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsDispatcher : MonoBehaviour
{
    public System.Action<GameObject> onInteract;
    public System.Action onTriggerHold;
    public System.Action onShoot;
    public System.Action onReload;
    public System.Action<EnemyNotification> onNotifyEnemies;

    public static EventsDispatcher Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            onTriggerHold?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, GameConstants.INTERACT_DISTANCE))
            {
                var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    onInteract?.Invoke(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            onReload?.Invoke();
        }
    }
}
