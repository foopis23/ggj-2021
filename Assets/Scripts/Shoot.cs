using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // public variables
    public GameObject HitParticle;

    // private variables
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 200);
            GameObject particleObject = Instantiate(HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
        }
    }
}
