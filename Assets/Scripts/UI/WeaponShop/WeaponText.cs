using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponText : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI priceText;

    public void SetText(string title, string price)
    {
        titleText.text = title;
        SetPriceText(price);
    }

    public void SetPriceText(string price)
    {
        priceText.text = price;
    }
}
