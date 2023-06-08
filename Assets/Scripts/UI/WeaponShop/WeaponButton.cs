using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public WeaponData weaponData;
    public Color equippedColor;

    protected WeaponText weaponText;
    protected Image buttonImage;
    protected Color initColor;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        weaponText = GetComponentInChildren<WeaponText>();
        weaponText.SetText(weaponData.title, weaponData.price.ToString());

        var button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        buttonImage = GetComponent<Image>();
        initColor = buttonImage.color;

        bool isEquipped = PlayerPrefsController.IsWeaponEquipped(weaponData.id);
        if (isEquipped)
            GetComponentInParent<WeaponShopView>().onWeaponEquipped?.Invoke(this);

        UpdateStatus();
    }

    protected virtual void OnButtonClicked()
    {
        if (!PlayerPrefsController.IsWeaponUnlocked(weaponData.id) && weaponData.price != 0)
        {
            if (!Buy())
                return;
        }
        Equip();
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
        bool isEquipped = PlayerPrefsController.IsWeaponEquipped(weaponData.id);
        if (isEquipped)
            return;
        GetComponentInParent<WeaponShopView>().onWeaponEquipped?.Invoke(this);
        PlayerPrefsController.EquipWeapon(weaponData.id, weaponData.title);
        Player.Instance.GetComponent<GunController>().UpdateWeapons();

        UpdateStatus();
    }

    public void UnEquip()
    {
        PlayerPrefsController.UnEquipWeapon(weaponData.id, weaponData.title);
        UpdateStatus();
    }

    protected virtual void UpdateStatus()
    {
        bool isUnlocked = PlayerPrefsController.IsWeaponUnlocked(weaponData.id);
        
        if (isUnlocked || weaponData.price == 0)
            weaponText.SetPriceText("");

        bool isEquipped = PlayerPrefsController.IsWeaponEquipped(weaponData.id);
        buttonImage.color = isEquipped ? equippedColor : initColor;
    }
}
