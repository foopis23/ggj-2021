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

    public float attackDistance;
    public int attackTimeMs;
    public int attackCoolDownMs;

    //state flags
    private bool isAgro;
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

        isAgro = true;
        isAttacking = false;
    }

    void Update()
    {
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAgro", isAgro);
        animator.SetBool("isAttackCoolingDown", isAttackCoolingDown);

        if (isAgro)
        {
            if (!isAttacking)
            {
                navMeshAgent.isStopped = false;
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
                navMeshAgent.isStopped = true;
            }

            //check if player is still close enough for agro
        }
        else
        {
            //wander
        }
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
}
