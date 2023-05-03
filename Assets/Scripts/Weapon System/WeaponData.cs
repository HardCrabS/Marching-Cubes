using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Gun weaponPrefab;
    public GameObject weaponPickupPrefab;

    [Header("Bullets")]
    public Projectile projectile;
    public float speed = 300f;
    public float msBetweenShots = 100;
    public int ammoMax = 30;
    public float reloadTime = 3f;
    public ParticleSystem impactFX;
}
