using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class Gun : MonoBehaviour
{
    private const float PREFIRE_WINDOW = 0.3f;

    // public variables;
    public LayerMask ShootLayerMask;
    public AudioSource shootSoundy;

    // private variables;
    private float lastFire;
    private bool preFire;
    private bool doThing;
    private Camera playerCamera;
    private Animator animator;
    private ParticleSystem fireParticle;

    // automatic properties
    public bool Empty { get; private set; }
    public GunData GunData { get; private set; }

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if(doThing)
        {
            doThing = false;
            Fire();
        }

        if(!Empty && Time.time > lastFire + GunData.FireCooldown)
        {
            animator.SetBool("isFiring", false);
            doThing = preFire;
            preFire = false;
        }
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
        shootSoundy.clip = gunData.FireSound;
        GameObject model = GunManager.Current.GetModel(gunData);
        animator = model.GetComponent<Animator>();
        fireParticle = model.GetComponentInChildren<ParticleSystem>();
    }

    public void Fire()
    {
        if(Time.time > lastFire + GunData.FireCooldown)
        {
            lastFire = Time.time;
            shootSoundy.volume = GunData.Model == "Assault" ? 0.5f : 1;
            shootSoundy.pitch = 1 + Random.value * 0.1f - 0.05f;
            shootSoundy.Play();
            animator.SetBool("isFiring", true);
            {
                if(fireParticle.isPlaying)
                {
                    fireParticle.Stop();
                }

                fireParticle.Play();
            }
            for(int i = 0; i < GunData.BulletsPerShot; i++)
            {
                RaycastHit hit;
                Quaternion randomSpread = Quaternion.Euler(
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f),
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f),
                    Random.value * GunData.Spread - (GunData.Spread * 0.5f)
                );

                bool success = Physics.Raycast(playerCamera.transform.position, randomSpread * playerCamera.transform.forward, out hit, GunData.BulletRange, ShootLayerMask);
                if(success)
                {
                    EventSystem.Current.FireEvent(new BulletHitCtx(hit, GunData.BulletDamage));
                    GameObject particleObject = Instantiate(GunData.HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
                    Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
                }
            }
        }
        else if(Time.time > lastFire + (GunData.FireCooldown * (1 - PREFIRE_WINDOW)))
        {
            preFire = true;
        }
    }
}
