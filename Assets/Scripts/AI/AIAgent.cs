using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    AIStates states;
    Animator anim;
    public GameObject[] wanderPoints;
    public int currentDestinationIndex = 0;

    void Start()
    {
        states = GetComponent<AIStates>();
        anim = GetComponent<Animator>();
        wanderPoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        FindNextPoint();

    }

    void Update()
    {
        if (states.isGettingHit)
        {
            states.currentState = AIStates.FSMStates.Attack;
        }
      
     

        switch (states.currentState)
        {
            case AIStates.FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case AIStates.FSMStates.Chase:
                UpdateChaseState();
                break;
            case AIStates.FSMStates.Attack:
                UpdateAttackState();
                break;
            
        }

    }

    void UpdatePatrolState()
    {
        print("patrol state");

        if (Vector3.Distance(transform.position, states.nextDestination) < 2)
        {FindNextPoint();}

        else if (states.distanceToPlayer <= states.chaseDistance && IsPlayedInClearFOV())
        {states.currentState = AIStates.FSMStates.Chase;}
        else if (states.distanceToPlayer <= states.attackDistance)
        {
            states.currentState = AIStates.FSMStates.Attack;
        }

      //  FaceTarget(states.nextDestination);
        states.agent.destination = states.nextDestination;
    }

    void UpdateChaseState()
    {
        print("chase state");
        states.nextDestination = states.player.transform.position;

        if (states.distanceToPlayer <= states.attackDistance)
        {
            states.currentState = AIStates.FSMStates.Attack;
        }
        else if (states.distanceToPlayer > states.chaseDistance && !IsPlayedInClearFOV())
        {
            states.currentState = AIStates.FSMStates.Patrol;
            FindNextPoint();

        }
        //  FaceTarget(states.nextDestination);
        states.agent.destination = states.nextDestination;
    }
    void UpdateAttackState()
    {
        print("attack state");

        states.isAttacking = true;
        states.agent.speed = 2;

        states.nextDestination = states.player.transform.position;

        if (states.distanceToPlayer > states.attackDistance && states.distanceToPlayer <= states.chaseDistance)
        {
            states.currentState = AIStates.FSMStates.Chase;
            states.agent.speed = states.agentSpeed;
        }
        else if (states.distanceToPlayer > states.chaseDistance && !IsPlayedInClearFOV())
        {
            states.agent.speed = states.agentSpeed;
            states.currentState = AIStates.FSMStates.Patrol;
            FindNextPoint();
            
        }
        FaceTarget(states.player.transform.position);
        states.agent.destination = states.nextDestination;

    }
    /*
    void UpdateHitState()
    {
        print("hit state");
        if (!states.isGettingHit) {
            if (states.distanceToPlayer <= states.attackDistance)
            {
                states.currentState = AIStates.FSMStates.Attack;
            }
            else if (states.distanceToPlayer > states.attackDistance)
            {
                states.currentState = AIStates.FSMStates.Chase;
            }
            else if (states.distanceToPlayer > states.chaseDistance)
            {
                states.currentState = AIStates.FSMStates.Patrol;
            }
        }
    }*/


    

    void FindNextPoint()
    {
        print(currentDestinationIndex);
        print(wanderPoints.Length);
        states.nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    bool IsPlayedInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = states.player.transform.position - states.enemyEyes.position;
        if (Vector3.Angle(directionToPlayer, states.enemyEyes.forward) <= states.fieldOfView)
        {
            if (Physics.Raycast(states.enemyEyes.position, directionToPlayer, out hit, states.chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    print("PlayerInSight");
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

}
