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

public class ZombieAI : EnemyAI
{
    //attack settings
    public float attackDistance; //distance the player can attack from
    public float groundPoundRadius; //the radius at which a ground pound attack lands a hit
    public float groundPoundDamage; //the amount of damage the ground pound attack will do
    public bool groundPoundLinearFalloff; //wether or not to drop the damage linearly based off the distance from the player


    //audio sources for each sound effect
    public AudioSource zombieAttackPrep1;
    public AudioSource zombieAttackPrep2;
    public AudioSource zombieAttackSmash;

    //the speed the ai is suppose to move at (pulled from the navagent comp)
    private float normalSpeed;

    //state flags
    private bool hasLastPlayerPos;
    private bool isAttacking;
    private bool isAttackCoolingDown;

    /*
    ....###....####....####.##....##.########.########.##....##.########..######.
    ...##.##....##......##..###...##....##....##.......###...##....##....##....##
    ..##...##...##......##..####..##....##....##.......####..##....##....##......
    .##.....##..##......##..##.##.##....##....######...##.##.##....##.....######.
    .#########..##......##..##..####....##....##.......##..####....##..........##
    .##.....##..##......##..##...###....##....##.......##...###....##....##....##
    .##.....##.####....####.##....##....##....########.##....##....##.....######.
    */

    void AttackPlayer()
    {
        lastSeenPlayerPos = target.position;
        lastSawPlayerTime = Time.time;
        hasLastPlayerPos = true;
        navMeshAgent.SetDestination(target.position);

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        // transform.LookAt(target.position, Vector3.up);
    }

    void FollowPlayer()
    {
        lastSeenPlayerPos = target.position;
        lastSawPlayerTime = Time.time;
        hasLastPlayerPos = true;
        navMeshAgent.SetDestination(target.position);

        //check if player is close enough to attack
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance && !isAttackCoolingDown)
        {
            isAttacking = true;
        }

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        // transform.LookAt(target.position, Vector3.up);
    }

    void LookForPlayer()
    {
        if (hasLastPlayerPos)
        {
            navMeshAgent.SetDestination(lastSeenPlayerPos);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }

        if (Time.time - lastSawPlayerTime >= lookForLostPlayerMs)
        {
            isAgro = false;
            hasLastPlayerPos = false;
        }
    }

    void Wander()
    {
        navMeshAgent.SetDestination(transform.position);
        //TODO: wander around
        hasLastPlayerPos = false;
        isAgro = canSeePlayer;
    }

    /*
    .##.....##.########..######...######.....###.....######...########..######.
    .###...###.##.......##....##.##....##...##.##...##....##..##.......##....##
    .####.####.##.......##.......##........##...##..##........##.......##......
    .##.###.##.######....######...######..##.....##.##...####.######....######.
    .##.....##.##.............##.......##.#########.##....##..##.............##
    .##.....##.##.......##....##.##....##.##.....##.##....##..##.......##....##
    .##.....##.########..######...######..##.....##..######...########..######.
    */

    public override void Init()
    {
        //intial properties
        lastSeenPlayerPos = new Vector3();
        normalSpeed = navMeshAgent.speed;
        health = maxHealth;

        //intial
        isAgro = false;
        hasLastPlayerPos = false;
        canSeePlayer = false;
        didSeePlayer = false;
        isAttacking = false;
        isAttackCoolingDown = false;
        navMeshAgent.updateRotation = false;
        invisible = false;

        //register event listener
        EventSystem.Current.RegisterEventListener<BulletHitCtx>(OnBulletHit);
    }

    public override void BehaviorTick()
    {
        //set animation states
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAgro", isAgro);

        if (isAgro)
        {
            AgroUpdate();
        }
        else
        {
            IdleUpdate();
        }
    }

    public override void FixedBehaviorTick()
    {

    }

    void AgroUpdate()
    {
        if (canSeePlayer)
        {
            if (!isAttacking)
            {
                FollowPlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            FollowPlayer();
        }
    }

    void IdleUpdate()
    {
        Wander();
    }

    /*
    .########.##.....##.########.##....##.########..######.
    .##.......##.....##.##.......###...##....##....##....##
    .##.......##.....##.##.......####..##....##....##......
    .######...##.....##.######...##.##.##....##.....######.
    .##........##...##..##.......##..####....##..........##
    .##.........##.##...##.......##...###....##....##....##
    .########....###....########.##....##....##.....######.
    */

    public void OnAttackCooldownFinished()
    {
        isAttackCoolingDown = false;
    }

    public void OnDamgeFinshed()
    {
        invisible = false;
        // foreach (MeshRenderer mesh in hurtMesh)
        // {
        //     mesh.material = normalMaterial;
        // }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPrepAttack1()
    {
        navMeshAgent.speed = attackChargeSpeed;
        zombieAttackPrep1.Play();
    }

    public void OnPrepAttack2()
    {
        zombieAttackPrep2.Play();
    }

    public void OnAttack()
    {
        navMeshAgent.speed = 0;
        zombieAttackSmash.Play();
        EventSystem.Current.FireEvent(new GroundPoundContext(new Vector3(transform.position.x, transform.position.y, transform.position.z), groundPoundRadius, groundPoundDamage, groundPoundLinearFalloff));
    }

    public void OnAttackFinished()
    {
        navMeshAgent.speed = normalSpeed;
        isAttacking = false;
        isAttackCoolingDown = true;
    }

    public void OnBulletHit(BulletHitCtx ctx)
    {
        if (gameObject.Equals(ctx.hit.collider.gameObject))
        {
            TakeDamage(ctx.damage);
        }
    }

    /*
    .##.....##.########.##.......########..########.########...######.
    .##.....##.##.......##.......##.....##.##.......##.....##.##....##
    .##.....##.##.......##.......##.....##.##.......##.....##.##......
    .#########.######...##.......########..######...########...######.
    .##.....##.##.......##.......##........##.......##...##.........##
    .##.....##.##.......##.......##........##.......##....##..##....##
    .##.....##.########.########.##........########.##.....##..######.
    */

    private void TakeDamage(float damage)
    {
        if (!invisible)
        {
            invisible = false;
            health -= damage;
            // foreach (MeshRenderer mesh in hurtMesh)
            // {
            //     mesh.material = hurtMaterial;
            // }
        }

        EventSystem.Current.CallbackAfter(OnDamgeFinshed, 400);
    }
}
