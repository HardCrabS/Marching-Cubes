using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTreasure : MonoBehaviour, IInteractable
{
    public int money = 300;
    public void Interact()
    {
        Wallet.Instance.AddMoney(money);
        Destroy(gameObject);
    }
}
