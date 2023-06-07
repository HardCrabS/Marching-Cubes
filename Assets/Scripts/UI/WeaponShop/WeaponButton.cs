using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public WeaponData weaponData;
    public Color equippedColor;

    WeaponText weaponText;
    Image buttonImage;
    Color initColor;

    private void Start()
    {
        weaponText = GetComponentInChildren<WeaponText>();
        weaponText.SetText(weaponData.title, weaponData.price.ToString());

        var button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        buttonImage = GetComponent<Image>();
        initColor = buttonImage.color;

        Debug.Log($"weapon [{weaponData.title}] unlocked: {PlayerPrefsController.IsWeaponUnlocked(weaponData.id)}");

        UpdateStatus();
    }

    void OnButtonClicked()
    {
        if (!PlayerPrefsController.IsWeaponUnlocked(weaponData.id) && weaponData.price != 0)
        {
            if (!Buy())
                return;
        }
        Equip();

        UpdateStatus();
    }

    bool Buy()
    {
        if (Wallet.Instance.HasEnoughMoney(weaponData.price))
        {
            Wallet.Instance.AddMoney(-weaponData.price);
            PlayerPrefsController.UnlockWeapon(weaponData.id);
            return true;
        }
        return false;
    }

    void Equip()
    {
        PlayerPrefsController.EquipWeapon(weaponData.id);
    }

    void UpdateStatus()
    {
        if (PlayerPrefsController.IsWeaponUnlocked(weaponData.id) || weaponData.price == 0)
        {
            weaponText.SetPriceText("");
        }

        buttonImage.color = PlayerPrefsController.IsWeaponEquipped(weaponData.id) ? equippedColor : initColor;
    }
}
