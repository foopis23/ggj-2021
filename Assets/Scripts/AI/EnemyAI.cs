using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class HurtMesh
{
    public MeshRenderer mesh;
    public Material hurtMaterial;
    public Material material;
}

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected Animator animator;

    // navigation settings
    [SerializeField] protected Transform target;
    [SerializeField] protected float viewDistance; //view distance for spotting player
    [SerializeField] protected float agroViewDistance; //view distance after agro'd
    [SerializeField] protected float viewConeAngle; //view cone angle for spotting player
    [SerializeField] protected float agroViewConeAngle; //view cone angle when player is spoitted
    [SerializeField] protected int lookForLostPlayerMs; //the amount of time the bot should look for a lost player
    [SerializeField] protected float timeBetweenSeePlayerChecks; //increaments to check on whether or not we can see the player
    [SerializeField] protected float rotationDamping; //controls ai rotation speed

    // navigation Properties
    protected float lastSawPlayerTime;
    protected Vector3 lastSeenPlayerPos;

    // attack settings
    protected bool invisible; //if the player is invisible this frame
    protected float attackChargeSpeed = 6.0f;

    // health settings
    [SerializeField] protected float maxHealth;
    protected float health;

    // animation stuff
    [SerializeField] protected HurtMesh[] hurtMesh; // list of meshes and materials to display when damaged and when not damaged

    // state flags
    protected bool isAgro;
    protected float lastPlayerSeenCheck;
    protected bool didSeePlayer;
    protected bool canSeePlayer;


    // Start is called before the first frame update
    private void Start()
    {
        //get nav mesh agent
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        //get player target
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        //get the animator
        if (animator == null)
            animator = GetComponent<Animator>();

        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        BehaviorTick();
    }

    private void FixedUpdate()
    {
        if (Time.time - lastPlayerSeenCheck > timeBetweenSeePlayerChecks)
        {
            didSeePlayer = canSeePlayer;
            canSeePlayer = DoesAISeePlayer();
        }

        FixedBehaviorTick();
    }

    protected bool DoesAISeePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        RaycastHit hit;

        float angle = (isAgro) ? agroViewConeAngle : viewConeAngle;
        float distance = (isAgro) ? agroViewDistance : viewDistance;

        lastPlayerSeenCheck = Time.time;

        return Mathf.Abs(Vector3.Angle(transform.position, target.position)) <= (angle / 2) &&
            Vector3.Distance(transform.position, target.position) <= distance &&
            Physics.Raycast(transform.position, direction, out hit) &&
            hit.collider.gameObject.tag == "Player" && !hit.collider.gameObject.GetComponent<Player>().Invisible;
    }
    
    public abstract void Init();
    public abstract void BehaviorTick();
    public abstract void FixedBehaviorTick();
}
