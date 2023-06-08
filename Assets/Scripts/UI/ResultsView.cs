using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultsView : MonoBehaviour
{
    [Header("General Stats")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI treasuresText;
    public TextMeshProUGUI accuracyText;

    [Header("Money")]
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI missionMoneyText;
    public TextMeshProUGUI enemiesMoneyText;
    public TextMeshProUGUI treasuresMoneyText;

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

    void SetStats()
    {
        float timeElapsed = Time.timeSinceLevelLoad;

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = timeString;

        var stats = StatsController.Instance;
        enemiesText.text = $"{stats.EnemiesKilled}/{stats.TotalEnemies} <size=30>x</size>";
        treasuresText.text = $"{stats.TreasuresCollected}/{stats.TotalTreasures} <size=30>x</size>";
        accuracyText.text = Random.Range(50, 100).ToString();
    }

    void CalculateMoney()
    {
        // TODO: move logic from view
        missionMoneyText.text = "250";
        int enemiesMoney = StatsController.Instance.EnemiesKilled;
        int treasuresMoney = (StatsController.Instance.TreasuresCollected * 300);
        int totalMoney = 250 + enemiesMoney + treasuresMoney;

        enemiesMoneyText.text = enemiesMoney.ToString();
        treasuresMoneyText.text = treasuresMoney.ToString();
        totalMoneyText.text = totalMoney.ToString();

        Wallet.Instance.AddMoney(totalMoney);
    }

    void ActivateResultsScreen()
    {
        if (isShown)
            return;

        Player.Instance.FreezeControls();

        SetStats();
        CalculateMoney();

        isShown = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        animator.SetTrigger("ShowResultsScreen");
    }
}
