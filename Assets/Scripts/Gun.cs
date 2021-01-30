using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class Gun : MonoBehaviour
{
    // public variables
    public string Name;
    public float BulletDamage;
    public float BulletRange;
    public float FireCooldown;
    public float Spread;
    public int BulletsPerShot;
    public bool Automatic;
    public GameObject HitParticle;

    // private variables;
    private float lastFire;
    private Camera playerCamera;

    void Start()
    {
        lastFire = 0f;
        playerCamera = Camera.main;
    }

    void Update()
    {

    }

    public void Fire()
    {
        if(Time.time > lastFire + FireCooldown)
        {
            lastFire = Time.time;
            for(int i = 0; i < BulletsPerShot; i++)
            {
                RaycastHit hit;
                bool success = Physics.Raycast(playerCamera.transform.position, Quaternion.Euler(Random.value * Spread - (Spread * 0.5f), Random.value * Spread - (Spread * 0.5f), Random.value * Spread - (Spread * 0.5f)) * playerCamera.transform.forward, out hit, BulletRange);
                if(success)
                {
                    EventSystem.Current.FireEvent(new BulletHitCtx(hit, BulletDamage));
                    GameObject particleObject = Instantiate(HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
                    Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
                }
            }
        }
    }
}
