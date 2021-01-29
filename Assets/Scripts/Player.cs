using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class PlayerHealthChangedCtx : EventContext
{
    public float dHealth;
    public float currentHealth;
    public Player player;

    public PlayerHealthChangedCtx(float dHealth, float currentHealth, Player player)
    {
        this.dHealth = dHealth;
        this.currentHealth = currentHealth;
        this.player = player;
    }
}

public class GotoNextLevelContext : EventContext
{
    public Vector3 location;
    
    public GotoNextLevelContext(Vector3 location)
    {
        this.location = location;
    }
}

public class Player : MonoBehaviour
{
    // public variables
    public LayerMask TerminalLayerMask;
    public GameObject HitParticle;
    public float maxHealth;

    // private variables
    private Camera playerCamera;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterEventListener<GroundPoundContext>(OnGroundPound);
        EventSystem.Current.RegisterEventListener<GotoNextLevelContext>(OnGotoNextLevel);
        playerCamera = Camera.main;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Input.GetButtonDown("Fire1"))
        {
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 200);
            GameObject particleObject = Instantiate(HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
        }

        if(Input.GetButtonDown("Interact"))
        {
            bool success = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3, TerminalLayerMask);
            if(success)
            {
                Terminal terminal = hit.collider.gameObject.GetComponent<Terminal>();
                terminal.HandleInteract();
            }
        }
    }

    void OnGroundPound(GroundPoundContext ctx)
    {
        //distance not including y
        Vector3 from = new Vector3(ctx.location.x, 0, ctx.location.z);
        Vector3 current = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(from, current);

        if (distance <= ctx.radius)
        {
            float damage = ctx.damage;
            if (ctx.linearFalloff)
            {
                damage *= (distance / ctx.radius);
            }

            TakeDamage(damage);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;

        EventSystem.Current.FireEvent(new PlayerHealthChangedCtx(damage, health, this));

        if (health == 0)
        {
            //perish
        }
    }

    public void OnGotoNextLevel(GotoNextLevelContext context)
    {
        Debug.Log("poopy stinker");
        transform.position = context.location + Vector3.up * 10;
    }
}
