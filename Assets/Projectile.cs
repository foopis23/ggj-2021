using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class ProjectileHitContext : EventContext {
    public float damage;
    public Vector3 velocity;
    public Collider hit;
    
    public ProjectileHitContext(float damage, Vector3 velocity, Collider hit) {
        this.damage = damage;
        this.velocity = velocity;
        this.hit = hit;
    }
}

public class Projectile : MonoBehaviour
{
    public float maxDistance;
    public float damage;
    public Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        transform.position += (velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        EventSystem.Current.FireEvent(new ProjectileHitContext(damage, velocity, other));
        Destroy(gameObject);
    }
}
