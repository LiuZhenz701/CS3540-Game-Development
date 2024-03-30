using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class MiaMovementHandler : MonoBehaviour
{
    MiaController miaController;
    MiaInputHandler miaInputHandler;


    public float velocityChange = 4;


    Vector3 curVelocity;
    Vector3 targetVelocity;
 

    Vector3 storeDirection;
    Vector3 prevDirection;
    Vector3 overrideDirection;

    bool overrideForce;


    bool applyJumpForce = false;

    Vector3 input, moveDirection;

    public void Init(MiaController mc, MiaInputHandler mih)
    {
        miaController = mc;
        miaInputHandler = mih;
    }


    public void Tick()
    {
        //if movement isnt being overrided, move regularly.
        if (!overrideForce)
        {
            MovementNormal();
            HandleAttacks();
            HandleJump();

        }
        //else override, and set movement inputs to 0
        else
        {
            miaController.horizontal = 0;
            miaController.vertical = 0;
            OverrideLogic();
        }
    }

    void MovementNormal()
    {
        Vector3 h = miaInputHandler.mainCamera.transform.right * miaController.horizontal;
        Vector3 v = miaInputHandler.mainCamera.transform.forward * miaController.vertical;
        // Set y to 0 if not in the air
        if (miaController.onGround && !miaController.isOnJumpPad)
        {
            h.y = 0;
            v.y = 0;
        }

        input = (h + v).normalized;

        if (miaController.isPunching || miaController.isKicking)
        {
            input = Vector3.zero;
        }
            

        if (h != Vector3.zero || v != Vector3.zero)
        {
            miaController.curState = MiaController.CharStates.moving;
        }

        HandleRotation_Normal(h, v);
        // Handle player rotation
        if (miaController.onLocomotion && miaController.onGround)
        {
            //HandleRotation_Normal(h, v);
        }

        // Handle player speed and movement input
        float targetSpeed = miaController.walkSpeed;
        if (miaController.isRunning && miaController.groundAngle == 0)
        {
            targetSpeed = miaController.sprintSpeed;
        }

        input *= targetSpeed;

        // Handle midair movement input
        if (!miaController.onGround)
        {
            moveDirection = Vector3.Lerp(moveDirection, input, miaController.airControl * Time.deltaTime);
            moveDirection.y -= miaController.gravity * Time.deltaTime;

        }

        // Handle gravity.
        input.y = moveDirection.y;

        miaController.controller.Move(input * Time.deltaTime);

        HandleAnimations_Normal();
    }


    //handle player rotation based on direction camera is pointing.
    void HandleRotation_Normal(Vector3 h, Vector3 v)
    { 
        Vector3 targetDirection = h + v;
        if (targetDirection.magnitude > 0)
        {
            // calc the target rotation to look towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);

            //  rotate toward the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, velocityChange * Time.deltaTime);

        }
    }

    void HandleAnimations_Normal()
    {
        Vector3 relativeDirection = transform.InverseTransformDirection(miaController.moveDirection);
        float h = relativeDirection.x;
        float v = relativeDirection.z;

        if(miaController.obstacleAhead)
        {
            v = 0;
        }
        
        miaController.anim.SetFloat(StaticVars.horizontal, h, .2f, Time.deltaTime);
        miaController.anim.SetFloat(StaticVars.vertical, v, .2f, Time.deltaTime);
    }

    void HandleJump()
    {
        // if Mia is grounded and can jump
        if (miaController.onGround && miaController.canJump)
        {
            // and if Mia recieved Jump Input, is not already jumping, is moving, isn't in the air, and jump hasn't been pressed already ->
            if (miaController.hasJumpInput 
                && !miaController.isJumping 
                && miaController.onLocomotion 
                && miaController.curState != MiaController.CharStates.inAir 
                && miaController.curState != MiaController.CharStates.hold
                )
            {
                //idle jumping
                if (miaController.curState == MiaController.CharStates.idle)
                {
                    miaController.isJumping = true;
                    miaController.canJump = false;
                    miaController.anim.SetBool(StaticVars.special, true);
                    miaController.anim.SetInteger(StaticVars.specialType, StaticVars.getAnimSpecialType(StaticVars.AnimSpecials.jump_idle));
                    //put Mia in "hold" state so jump button can't be pressed again
                    miaController.curState = MiaController.CharStates.hold;
                    miaController.anim.SetBool(StaticVars.inAir, true);
                    miaController.canJump = false;
                }

                //moving jumping
                if (miaController.curState == MiaController.CharStates.moving)
                {
                    //determine which leg is forward in order to set jump animation correctly.
                    miaController.LegFront();
                    miaController.isJumping = true;
                    miaController.anim.SetBool(StaticVars.special, true);
                    miaController.anim.SetInteger(StaticVars.specialType, StaticVars.getAnimSpecialType(StaticVars.AnimSpecials.run_jump));

                    //put Mia in "hold" state so jump button can't be pressed again
                    miaController.curState = MiaController.CharStates.hold;
                    miaController.anim.SetBool(StaticVars.inAir, true);
                    miaController.canJump = false;
                }
            }
        }

        if (miaController.isJumping)
        {
            // if mia is on the ground and no jump force has been applied, its the beginning of the jump
            if (miaController.onGround)
            {
                if(!applyJumpForce)
                {
                    applyJumpForce = true;
                    StartCoroutine(AddJumpForce(0));

                }
            } 
            // else mia just landed
            else
            {
                miaController.isJumping = false;
            }
        }
    }

    void OverrideLogic()
    {
        moveDirection = Vector3.zero;
        input = Vector3.zero;
    }

    IEnumerator AddJumpForce(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveDirection.y = Mathf.Sqrt(2 * miaController.jumpForce * miaController.gravity);
        miaController.controller.Move(moveDirection * Time.deltaTime);
        StartCoroutine(CloseJump());

    }
    IEnumerator CloseJump()
    {
        yield return new WaitForSeconds(.3f);
        miaController.curState = MiaController.CharStates.inAir;
        miaController.isJumping = false;
        applyJumpForce = false;
        miaController.canJump = false;
        StartCoroutine(ReEnableJump());
    }

    IEnumerator ReEnableJump()
    {
        yield return new WaitForSeconds(1.3f);
        miaController.canJump = true;
    }
    private void HandleAttacks()
    {
    
        if (miaController.hasPunchInput)
        {
            // If not in punch combo, start combo
            if (!miaController.isPunching)
            {
                miaController.isPunching = true;
                miaController.anim.SetBool(StaticVars.punchOne, true);
                //miaController.anim.SetBool(StaticVars.punchComboOver, false);
                StartCoroutine(EndPunchState(.4f));
            }
            // Else continue punch combo
            //else
            //{
             //   miaController.inPunchCombo = true;
              //  miaController.anim.SetBool(StaticVars.punchOne, true);
              //  StartCoroutine(EndPunchState(.5f));
            //}
            
        }


        if (miaController.hasKickInput)
        {
            // If not in kick combo, start kick combo
            if (!miaController.isKicking)
            {
                miaController.isKicking = true;
                miaController.anim.SetBool(StaticVars.kickOne, true);
                //miaController.anim.SetBool(StaticVars.kickComboOver, false);
                StartCoroutine(EndKickState(.4f));
            }
            // Else continue kick combo
         //   else
         //   {
          //      miaController.isKicking = true;
           //     miaController.anim.SetBool(StaticVars.kickOne, true);
          //      StartCoroutine(EndKickState(miaController.anim.GetCurrentAnimatorStateInfo(0).length));
           // }
            
        }
    }

    // Coroutine to delay the transition out of punch state
    IEnumerator EndPunchState(float duration)
    {
        yield return new WaitForSeconds(duration);

        EndPunchAnimation();
    }

    // Coroutine to delay the transition out of kick state
    IEnumerator EndKickState(float duration)
    {
        yield return new WaitForSeconds(duration);

        EndKickAnimation();
    }

    // Method to end punch animation after delay
    void EndPunchAnimation()
    {
        miaController.anim.SetBool(StaticVars.punchOne, false);
       // miaController.anim.SetBool(StaticVars.punchComboOver, true);
        miaController.isPunching = false;
    }

    // Method to end kick animation after delay
    void EndKickAnimation()
    {
        miaController.anim.SetBool(StaticVars.kickOne, false);
        //miaController.anim.SetBool(StaticVars.kickComboOver, true);
        miaController.isKicking = false;
    }









}
