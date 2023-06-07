using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsView : MonoBehaviour
{
    Animator animator;
    Button continueButton;
    bool isShown = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        continueButton = GetComponentInChildren<Button>();
        continueButton.onClick.AddListener(() => { SceneManager.LoadScene("Lobby"); });
        EventsDispatcher.Instance.onPlayerDead += ActivateResultsScreen;
        EventsDispatcher.Instance.onLevelFinished += ActivateResultsScreen;
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
        if (isShown)
            return;

        Player.Instance.FreezeControls();

        isShown = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        animator.SetTrigger("ShowResultsScreen");
    }
}
