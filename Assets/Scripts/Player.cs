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

public class BulletHitCtx : EventContext
{
    public RaycastHit hit;
    public float damage;

    public BulletHitCtx(RaycastHit hit, float damage)
    {
        this.hit = hit;
        this.damage = damage;
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
    public LayerMask InteractionLayerMask;
    public GameObject HitParticle;
    public int InteractionDistance = 2;
    public float maxHealth;

    // private variables
    private Camera playerCamera;
    private float health;

    // automatic properties
    public CharacterController CharacterController { get; set; }
    public CPMPlayer CPMPlayer { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterEventListener<GroundPoundContext>(OnGroundPound);
        EventSystem.Current.RegisterEventListener<GotoNextLevelContext>(OnGotoNextLevel);
        playerCamera = Camera.main;
        health = maxHealth;
        CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // shot
        if(Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 200))
            {
                EventSystem.Current.FireEvent(new BulletHitCtx(hit, 10));
            }
            GameObject particleObject = Instantiate(HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            Destroy(particleObject, particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier);
        }

        // check for interactables
        bool success = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, InteractionDistance, InteractionLayerMask);
        if(success)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            interactable.OnInteractable();
            if(Input.GetButtonDown("Interact"))
            {
                interactable.Interact();
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
        CharacterController.enabled = false;
        transform.position = context.location;
        CharacterController.enabled = true;
    }
}
