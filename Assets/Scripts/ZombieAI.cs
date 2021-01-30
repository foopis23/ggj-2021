using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CallbackEvents;

public class GroundPoundContext : EventContext
{
    public Vector3 location;
    public float radius;
    public float damage;
    public bool linearFalloff;

    public GroundPoundContext(Vector3 location, float radius, float damage, bool linearFalloff)
    {
        this.location = location;
        this.radius = radius;
        this.damage = damage;
        this.linearFalloff = linearFalloff;
    }
}

public class ZombieAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navMeshAgent;
    public Animator animator;

    public GameObject meshWrapper;

    public float viewDistance;
    public float agroViewDistance;
    public float viewConeAngle;
    public float agroViewConeAngle;
    public float attackDistance;
    public int attackTimeMs;
    public int attackCoolDownMs;
    public int lookForLostPlayerMs;
    public int timeToSpotMS;
    public float timeBetweenSeePlayerChecks;
    public float rotationDamping;
    public float groundPoundRadius;
    public float groundPoundDamage;
    public bool groundPoundLinearFalloff;
    public bool invisible;

    public MeshRenderer[] hurtMesh;
    public Material hurtMaterial;
    public Material normalMaterial;

    public float maxHealth;

    private Vector3 lastSeenPlayerPos;
    private float lastPlayerSeenCheck;
    private float health;

    public AudioSource zombieAttackPrep1;
    public AudioSource zombieAttackPrep2;
    public AudioSource zombieAttackSmash;

    //state flags
    private bool isAgro;
    private bool hasLastPlayerPos;
    private bool didSeePlayer;
    private bool canSeePlayer;
    private bool isAttacking;
    private bool isAttackCoolingDown;


    void Start()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        lastSeenPlayerPos = new Vector3();
        isAgro = false;
        hasLastPlayerPos = false;
        canSeePlayer = false;
        didSeePlayer = false;
        isAttacking = false;
        isAttackCoolingDown = false;
        navMeshAgent.updateRotation = false;
        invisible = false;

        health = maxHealth;

        EventSystem.Current.RegisterEventListener<BulletHitCtx>(OnBulletHit);
    }

    void Update()
    {
        //set animation set
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAgro", isAgro);
        animator.SetBool("isAttackCoolingDown", isAttackCoolingDown);

        if (isAgro)
        {
            if (canSeePlayer)
            {
                //chase and attack player
                lastSeenPlayerPos = target.position;
                hasLastPlayerPos = true;
                navMeshAgent.SetDestination(target.position);

                if (!isAttacking)
                {
                    //check if player is close enough to attack
                    if (navMeshAgent.remainingDistance <= attackDistance && !isAttackCoolingDown)
                    {
                        isAttacking = true;
                        EventSystem.Current.CallbackAfter(OnAttackFinished, attackTimeMs);
                    }
                }

                Vector3 lookPos = target.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
                // transform.LookAt(target.position, Vector3.up);

            }
            else
            {
                //trigger a countdown for when to stop looking for the player
                if (didSeePlayer)
                {
                    EventSystem.Current.CallbackAfter(OnLookingForPlayer, lookForLostPlayerMs);
                }

                if (hasLastPlayerPos)
                {
                    navMeshAgent.SetDestination(lastSeenPlayerPos);
                }
                else
                {
                    navMeshAgent.SetDestination(transform.position);
                }

            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            //wander around
            hasLastPlayerPos = false;
            isAgro = canSeePlayer;
        }
    }

    void FixedUpdate()
    {
        if (Time.time - lastPlayerSeenCheck > timeBetweenSeePlayerChecks)
        {
            didSeePlayer = canSeePlayer;
            canSeePlayer = doesAISeePlayer();
        }
    }

    bool doesAISeePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        RaycastHit hit;

        float angle = (isAgro) ? agroViewConeAngle : viewConeAngle;
        float distance = (isAgro) ? agroViewDistance : viewDistance;

        lastPlayerSeenCheck = Time.time;

        return Mathf.Abs(Vector3.Angle(transform.position, target.position)) <= (angle / 2) &&
            Vector3.Distance(transform.position, target.position) <= distance &&
            Physics.Raycast(transform.position, direction, out hit) &&
            hit.collider.gameObject.tag == "Player";
    }

    void OnAttackCooldownFinished()
    {
        isAttackCoolingDown = false;
    }

    void OnLookingForPlayer()
    {
        isAgro = canSeePlayer;
    }

    private void takeDamage(float damage)
    {
        if (!invisible)
        {
            invisible = true;
            health -= damage;
            foreach (MeshRenderer mesh in hurtMesh)
            {
                mesh.material = hurtMaterial;
            }
        }

        EventSystem.Current.CallbackAfter(OnDamgeFinshed, 400);
    }

    private void OnDamgeFinshed()
    {
        invisible = false;
        foreach (MeshRenderer mesh in hurtMesh)
        {
            mesh.material = normalMaterial;
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnPrepAttack1()
    {
        zombieAttackPrep1.Play();
    }

    private void OnPrepAttack2()
    {
        zombieAttackPrep2.Play();
    }

    private void OnAttack()
    {
        zombieAttackSmash.Play();
        EventSystem.Current.FireEvent(new GroundPoundContext(new Vector3(transform.position.x, transform.position.y, transform.position.z), groundPoundRadius, groundPoundDamage, groundPoundLinearFalloff));
    }

    private void OnAttackFinished()
    {
        isAttackCoolingDown = true;
        isAttacking = false;
        EventSystem.Current.CallbackAfter(OnAttackCooldownFinished, attackCoolDownMs);
    }

    void OnBulletHit(BulletHitCtx ctx)
    {
        if (gameObject.Equals(ctx.hit.collider.gameObject))
        {
            takeDamage(ctx.damage);
        }
    }
}
