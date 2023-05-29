using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoView : MonoBehaviour
{
    TextMeshProUGUI ammoText;

    void Start()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
        Player.Instance.onAmmoUpdated += UpdateAmmoText;
    }

    void UpdateAmmoText(int ammo, int maxAmmo)
    {
        ammoText.text = $"{ammo}/{maxAmmo}";
    }
}
