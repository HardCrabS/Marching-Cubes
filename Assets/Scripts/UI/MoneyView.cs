using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyView : MonoBehaviour
{
    TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();

        var money = Wallet.Instance.GetMoney();
        UpdateText(money);

        EventsDispatcher.Instance.onMoneyUpdated += UpdateText;
    }

    void UpdateText(int money)
    {
        moneyText.text = money.ToString();
    }
}
