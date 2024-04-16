using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static TPC.MiaController;

public class AIStates : MonoBehaviour
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

    [Header("Info")]
    public bool isDead = false;
    public FSMStates currentState;


    [Header("Stats")]
    public int currentHealth;
    public float agentSpeed;
    public float chaseDistance = 10;
    public float attackDistance = 5;
    public float elapsedTime = 0;
    public int fieldOfView = 90;


    public Vector3 nextDestination;
    public float distanceToPlayer;

 

    [Header("States")]
    public bool isGettingHit;
    public bool isAttacking;


    #region StateRequests
    [Header("State Requests")]

    #endregion

    #region References
    [Header("References")]
    public GameObject player;
    AIRagdoll ragdoll;
    GameObject levelManager;
    HealthControl healthControl;
    SkinnedMeshRenderer[] skinnedMeshRenderers;
    public NavMeshAgent agent;
    public Transform enemyEyes;
    Animator anim;
    public GameObject punchVFX;
    public AudioClip grannyScreamSFX;
    public float screamInterval = 10f;
    #endregion


    void Start()
    {
        //obtain references
        ragdoll = GetComponent<AIRagdoll>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        healthControl = levelManager.GetComponent<HealthControl>();
        currentHealth = healthControl.enemyHP;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;

        //enable hitboxes:
        EnableHitboxes();

        //init state machine:
        currentState = FSMStates.Patrol;

    }


    void Update()
    {
        currentHealth = healthControl.enemyCurHP;
        if (currentHealth <= 0)
        {
            currentState = FSMStates.Dead;
            Die();
        }
        BlinkWhenHitEffect();
        isGettingHit = AIHitbox.gotHitAlready;

        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        elapsedTime += Time.deltaTime;

       
        if (elapsedTime >= screamInterval)
        {
            AudioSource.PlayClipAtPoint(grannyScreamSFX, Camera.main.transform.position, 2f);
            elapsedTime = 0f;
        }
    }

  

    private void Die()
    {
        isDead = true;
        ragdoll.ActivateRagdoll();
    }

    private void EnableHitboxes()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            if (rb.gameObject.tag != "Granny")
            {
                AIHitbox hitBox = rb.gameObject.AddComponent<AIHitbox>();
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }
        var colliders = GetComponentsInChildren<CapsuleCollider>();
        foreach (var c in colliders)
        {
            if (c.gameObject.tag != "Granny")
            {
                c.isTrigger = true;
            }

        }
    }


    private void BlinkWhenHitEffect()
    {
        //glow when hit effect
        //print(AIHitbox.blinkTimer);
        AIHitbox.blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(AIHitbox.blinkTimer / AIHitbox.blinkDuration);
        float intensity = lerp * AIHitbox.blinkIntensity + 1f;

        foreach (var smr in skinnedMeshRenderers)
        {
            smr.material.color = Color.white * intensity;
        }
    }


    private void OnDrawGizmos()
    {
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * .5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * .5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }
}
