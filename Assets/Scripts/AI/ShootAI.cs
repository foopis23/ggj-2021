using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CallbackEvents;

public class ShootAI : EnemyAI
{
    //attack settings
    public float minStrafeDistance;
    public float maxStrafeDistance;
    public float maxProjectileDistance;
    public float projectileDamage;
    public float projectileSpeed;

    public GameObject projectilePrefab;


    //audio sources for each sound effect
    public AudioSource attackPrep1Sound;
    public AudioSource attackSound;

    //the speed the ai is suppose to move at (pulled from the navagent comp)
    private float normalSpeed;

    //state flags
    private bool isStrafing;
    private bool movingCloser;
    private bool movingAway;
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

    void Strafe()
    {
        navMeshAgent.SetDestination(transform.position);

        if (!isAttacking && !isAttackCoolingDown)
        {
            isAttacking = true;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < minStrafeDistance)
        {
            movingCloser = false;
            movingAway = true;
            isStrafing = false;
        }
        else if (distance > maxStrafeDistance)
        {
            movingCloser = true;
            movingAway = false;
            isStrafing = false;
        }
        else if (Mathf.Abs(maxStrafeDistance - distance) < 0.3)
        {
            movingCloser = false;
            movingAway = false;
            isStrafing = true;
        }
        else if (distance > minStrafeDistance && distance < maxStrafeDistance)
        {
            float ran = Random.Range(0, 1);
            if (ran < 0.6)
            {
                movingCloser = false;
                movingAway = false;
                isStrafing = true;
            }
        }

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }

    void FollowPlayer()
    {
        lastSeenPlayerPos = target.position;
        lastSawPlayerTime = Time.time;
        hasLastPlayerPos = true;
        navMeshAgent.SetDestination(target.position);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < minStrafeDistance)
        {
            movingCloser = false;
            movingAway = true;
            isStrafing = false;
        }
        else if (distance > maxStrafeDistance)
        {
            movingCloser = true;
            movingAway = false;
            isStrafing = false;
        }
        else if (Mathf.Abs(maxStrafeDistance - distance) < 0.3)
        {
            movingCloser = false;
            movingAway = false;
            isStrafing = true;
        }
        else if (distance > minStrafeDistance && distance < maxStrafeDistance)
        {
            float ran = Random.Range(0, 1);
            if (ran < 0.6)
            {
                movingCloser = false;
                movingAway = false;
                isStrafing = true;
            }
        }

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }

    void MoveAwayFromPlayer()
    {
        if (!isAttacking && !isAttackCoolingDown)
        {
            isAttacking = true;
        }

        lastSeenPlayerPos = target.position;
        lastSawPlayerTime = Time.time;
        hasLastPlayerPos = true;
        Vector3 hello = transform.position - transform.forward;
        navMeshAgent.SetDestination(hello);

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < minStrafeDistance)
        {
            movingCloser = false;
            movingAway = true;
            isStrafing = false;
        }
        else if (distance > maxStrafeDistance)
        {
            movingCloser = true;
            movingAway = false;
            isStrafing = false;
        }
        else if (Mathf.Abs(maxStrafeDistance - distance) < 0.3)
        {
            movingCloser = false;
            movingAway = false;
            isStrafing = true;
        }
        else if (distance > minStrafeDistance && distance < maxStrafeDistance)
        {
            float ran = Random.Range(0, 1);
            if (ran < 0.6)
            {
                movingCloser = false;
                movingAway = false;
                isStrafing = true;
            }
        }
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

    void AgroUpdate()
    {
        if (!isStrafing && !movingCloser && !movingAway)
        {
            movingCloser = true;
        }

        if (canSeePlayer)
        {
            if (isStrafing)
            {
                Strafe();
            }
            else if (movingCloser)
            {
                FollowPlayer();
            }
            else if (movingAway)
            {
                MoveAwayFromPlayer();
            }
        }
        else
        {
            LookForPlayer();
        }
    }

    void IdleUpdate()
    {
        Wander();
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
        foreach (HurtMesh obj in hurtMesh)
        {
            MeshRenderer mesh = obj.mesh;
            mesh.material = obj.material;
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPrepAttack()
    {
        attackPrep1Sound.Play();
        //TODO: Player sound
    }

    public void OnAttack()
    {
        attackSound.Play();
        //TODO: Shoot your goo my dood

        GameObject ob = Instantiate(projectilePrefab);
        ob.transform.position = transform.position + transform.forward + (transform.up * 1.35f);
        Projectile projectile = ob.GetComponent<Projectile>();
        projectile.velocity = transform.forward * projectileSpeed;
        projectile.damage = projectileDamage;
        isAttackCoolingDown = true;
        isAttacking = false;
    }

    public void OnAttackFinished()
    {
        isAttacking = false;
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
            foreach (HurtMesh obj in hurtMesh)
            {
                MeshRenderer mesh = obj.mesh;
                mesh.material = obj.hurtMaterial;
            }
        }

        EventSystem.Current.CallbackAfter(OnDamgeFinshed, 400);
    }
}
