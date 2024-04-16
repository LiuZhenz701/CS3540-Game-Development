using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class AIAgentAnimHandler : MonoBehaviour
{
    AIStates states;
    Animator anim;
    public static bool gotHitTrigger = false;
    bool attackTrigger = true;
    public static bool isAttackPlaying = false;
    public static bool isHitPlaying = false;
    private Vector3 startPosition;
    float jumpHeight = 3f;
    float jumpDuration = 3.11f;
    public AudioClip grannyHitSFX;


    void Start()
    {
        states = GetComponent<AIStates>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        

        switch (states.currentState)
        {
           
            case AIStates.FSMStates.Patrol:
                AnimPatrolState();
                break;

            case AIStates.FSMStates.Chase:
                AnimChaseState();
                break;
            case AIStates.FSMStates.Attack:
                AnimAttackState();
                break;
            
            case AIStates.FSMStates.Hit:
                AnimHitState();
                break;
            case AIStates.FSMStates.Dead:
                AnimDeadState();
                break;
        }

    }



    private void AnimPatrolState()
    {
        anim.SetInteger("State", 0);
        anim.SetFloat("Speed", states.agent.velocity.magnitude);
    }

    private void AnimChaseState()
    {
        anim.SetInteger("State", 1);

        anim.SetFloat("Speed", states.agent.velocity.magnitude);
    }

    private void AnimAttackState()
    {
        anim.SetInteger("State", 2);

        int attackNum = UnityEngine.Random.Range(0, 2);

        if (attackTrigger && !isAttackPlaying)
        {
            float duration = .5f;
            if (attackNum == 0)
            {
                startPosition = transform.position; 
           //     DoJump();
                duration = jumpDuration;
            }
            anim.SetInteger("Attack", attackNum);
            attackTrigger = false;
            isAttackPlaying = true;
            states.punchVFX.SetActive(true); 
            StartCoroutine(Attack(duration));
        }
    }
    IEnumerator Attack(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttackPlaying = false;
        attackTrigger = true;
        states.punchVFX.SetActive(false);

    }
  



    private void AnimHitState()
    {
        anim.SetInteger("State", 3);

        if (!isHitPlaying)
        {
            AudioSource.PlayClipAtPoint(grannyHitSFX, Camera.main.transform.position, .25f);

            anim.SetInteger("Hit", 0);
            isHitPlaying = true;
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(.5f);
        isHitPlaying = false;
    }

    private void AnimDeadState()
    {
       // throw new NotImplementedException();
    }

}
