using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsView : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EventsDispatcher.Instance.onPlayerDead += ActivateResultsScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ActivateResultsScreen();
        }
    }

    void ActivateResultsScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        animator.SetTrigger("ShowResultsScreen");
    }
}
