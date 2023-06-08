using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorButton : WeaponButton
{
    public ArmorData armorData;

    protected override void Init()
    {
        weaponText = GetComponentInChildren<WeaponText>();
        weaponText.SetText(armorData.title, armorData.price.ToString());

        var button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        buttonImage = GetComponent<Image>();
        initColor = buttonImage.color;

        UpdateStatus();
    }

    protected override void OnButtonClicked()
    {
        if (!Wallet.Instance.HasEnoughMoney(armorData.price))
            return;
        bool isEquipped = PlayerPrefsController.GetEquippedArmor().StartsWith(armorData.title);
        if (isEquipped)
            return;

        Wallet.Instance.AddMoney(-armorData.price);
        PlayerPrefsController.EquipArmor(armorData.title);

        UpdateStatus();
    }

    protected override void UpdateStatus()
    {
        bool isEquipped = PlayerPrefsController.GetEquippedArmor().StartsWith(armorData.title);
        buttonImage.color = isEquipped ? equippedColor : initColor;
    }
}
