using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Gun weaponPrefab;
    public GameObject weaponPickupPrefab;
}
