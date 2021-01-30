using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class Gun : MonoBehaviour
{
    // public variables;
    public AudioSource shootSoundy;

    // private variables;
    private float lastFire;
    private Camera playerCamera;

    // automatic properties
    public bool Empty { get; private set; }
    public GunData GunData { get; private set; }

    void Start()
    {
        playerCamera = Camera.main;
    }

    public void Reset()
    {
        lastFire = 0f;
        Empty = true;
    }

    public void SetData(GunData gunData)
    {
        lastFire = 0f;
        Empty = false;
        GunData = gunData;
    }

    public void Fire()
    {
        if(Time.time > lastFire + GunData.FireCooldown)
        {
            lastFire = Time.time;
            shootSoundy.Play();
            for(int i = 0; i < GunData.BulletsPerShot; i++)
            {
                RaycastHit hit;
                Quaternion randomSpread = Quaternion.Euler(
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f),
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f),
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f)
                );

                bool success = Physics.Raycast(playerCamera.transform.position, randomSpread * playerCamera.transform.forward, out hit, GunData.BulletRange);
                if(success)
                {
                    EventSystem.Current.FireEvent(new BulletHitCtx(hit, GunData.BulletDamage));
                    GameObject particleObject = Instantiate(GunData.HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
                    Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
                }
            }
        }
    }
}
