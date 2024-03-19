using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;
    float elapsedTime = 0;

    public float enemySpeed = 5;
    public float chaseDistance = 10;
    public GameObject player;
    public float attackDistance = 5;


    EnemyHealth enemyHealth;
    int health;
    Transform deadTransform;
    bool isDead;

    Animator anim;
    int currentDestinationIndex = 0;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        anim = GetComponent<Animator>();

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;

        isDead = false;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch(currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            /**
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            */
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }
        elapsedTime += Time.deltaTime;

        if(health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();

    }

    void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);

        if(Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        /*
        else if(distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        */
        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

    }
    void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);
        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }
        FaceTarget(nextDestination);

        //transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }
    void UpdateAttackState()
    {
        print("Attacking!");
        anim.SetInteger("animState", 2);
        nextDestination = player.transform.position;

        if(distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if(distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if(distanceToPlayer > chaseDistance)
        {
            currentState= FSMStates.Patrol;
        }
        FaceTarget(nextDestination);
        anim.SetInteger("animState", 3);
    }
    void UpdateDeadState()
    {
        anim.SetInteger("animState", 4);
        deadTransform = gameObject.transform;
        isDead = true;
        Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
