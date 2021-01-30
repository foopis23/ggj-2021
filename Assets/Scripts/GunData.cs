using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GunData", menuName = "ScriptableObjects/GunData", order = 1)]
public class GunData : ScriptableObject
{
    public string Model;
    public float BulletDamage;
    public float BulletRange;
    public float FireCooldown;
    public float Spread;
    public int BulletsPerShot;
    public bool Automatic;
    public AudioClip FireSound;
    public GameObject HitParticle;
}
