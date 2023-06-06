using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance;

    int money = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            AddMoney(100);
    }

    public int GetMoney() => money;

    public bool HasEnoughMoney(int value)
    {
        return money >= value;
    }

    public void AddMoney(int value)
    {
        money += value;
        if (money < 0)
            money = 0;

        Save();
    }

    void Save()
    {
        EventsDispatcher.Instance.onMoneyUpdated?.Invoke(money);
        PlayerPrefsController.SetMoney(money);
        Debug.Log($"Saved {money} money");
    }

    void Load()
    {
        money = PlayerPrefsController.GetMoney();
        EventsDispatcher.Instance.onMoneyUpdated?.Invoke(money);
        Debug.Log($"Loaded {money} money");
    }
}
