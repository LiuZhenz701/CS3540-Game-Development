using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Hit,
        Dead
    }

    public FSMStates currentState;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;
    float elapsedTime = 0;

    public float enemySpeed = 5;
    public float chaseDistance = 10;
    public float grandmaRunSpeed = 5;
    public GameObject player;
    public float attackDistance = 5;
    public GameObject levelManagerObject;
    bool isYelling;
    bool isGettingHit;

    HealthControl healthControl;
    int health;
    Transform deadTransform;
    bool isDead;

    Animator anim;
    int currentDestinationIndex = 0;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wanderPoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        anim = GetComponent<Animator>();

        GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        healthControl = levelManager.GetComponent<HealthControl>();

        isDead = false;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        health = healthControl.enemyCurHP;
        IsHit();
        Debug.Log(health);

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
        
        
        switch (currentState)
        {

            case FSMStates.Patrol:
                UpdatePatrolState();
                break;

            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;

            case FSMStates.Dead:
                UpdateDeadState();
                break;
            case FSMStates.Hit:
                UpdateHitState();
                break;
        }
            
        

        elapsedTime += Time.deltaTime;
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
        
        else if(distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        
        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

    }
    void UpdateChaseState()
    {
        anim.SetInteger("animState", 3);
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

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, grandmaRunSpeed * Time.deltaTime);
    }
    void UpdateAttackState()
    {
        print("Attacking!");
        
        isYelling = true;
        anim.SetInteger("animState", 6);
        nextDestination = player.transform.position;
        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }
        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
        

        FaceTarget(nextDestination);
    }

    void UpdateHitState()
    {
        
        anim.SetInteger("animState", 5);

        TakeKnockback takeKnockback = GetComponent<TakeKnockback>();

        takeKnockback.ApplyKnockback(player);
        bool playerIsAttacking = player.GetComponent<MiaController>().isPunching || player.GetComponent<MiaController>().isKicking;
        
        if (distanceToPlayer <= attackDistance && !playerIsAttacking)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && !playerIsAttacking)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }
        
        

    }

    void IsHit()
    {
        bool playerIsAttacking = player.GetComponent<MiaController>().isPunching || player.GetComponent<MiaController>().isKicking;
        if  (distanceToPlayer <= attackDistance && playerIsAttacking)
        {
            currentState = FSMStates.Hit;
            healthControl.enemyHit(1);
            if (!isGettingHit)
            {
                isGettingHit = true;
                StartCoroutine(EndGettingHit());
            }
            
        }
 
    }
    IEnumerator EndGettingHit()
    {
        yield return new WaitForSeconds(1.5f);
        isGettingHit = false;
    }
    void UpdateDeadState()
    {
        anim.SetInteger("animState", 4);
        deadTransform = gameObject.transform;
        isDead = true;
        GameObject levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");
        levelManagerObject.GetComponent<LevelManager>().GameWin();
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
