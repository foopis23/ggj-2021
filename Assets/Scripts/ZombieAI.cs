using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CallbackEvents;

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

    private Vector3 lastSeenPlayerPos;

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
    }

    void Update()
    {
        //reset some stuff
        navMeshAgent.updatePosition = true;

        //set animation set
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAgro", isAgro);
        animator.SetBool("isAttackCoolingDown", isAttackCoolingDown);

        didSeePlayer = canSeePlayer;
        canSeePlayer = doesAISeePlayer();

        if (isAgro)
        {
            if (canSeePlayer)
            {
                //chase and attack player
                lastSeenPlayerPos = target.position;
                hasLastPlayerPos = true;

                if (!isAttacking)
                {
                    navMeshAgent.SetDestination(target.position);

                    //check if player is close enough to attack
                    if (navMeshAgent.remainingDistance <= attackDistance && !isAttackCoolingDown)
                    {
                        isAttacking = true;
                        EventSystem.Current.CallbackAfter(OnAttackFinished, attackTimeMs);
                    }
                }
                else
                {
                    navMeshAgent.updatePosition = false;
                }
            }else
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
            //wander around
            hasLastPlayerPos = false;
            isAgro = canSeePlayer;
        }
    }

    bool doesAISeePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        RaycastHit hit;

        float angle = (isAgro) ? agroViewConeAngle : viewConeAngle;
        float distance = (isAgro) ? agroViewDistance : viewDistance;

        return Mathf.Abs(Vector3.Angle(transform.position, target.position)) <= (angle / 2) &&
            Vector3.Distance(transform.position, target.position) <= distance &&
            Physics.Raycast(transform.position, direction, out hit) &&
            hit.collider.gameObject.tag == "Player";
    }

    void OnAttackFinished()
    {
        isAttackCoolingDown = true;
        isAttacking = false;
        EventSystem.Current.CallbackAfter(OnAttackCooldownFinished, attackCoolDownMs);
    }

    void OnAttackCooldownFinished()
    {
        isAttackCoolingDown = false;
    }

    void OnLookingForPlayer()
    {
        isAgro = canSeePlayer;
    }
}
