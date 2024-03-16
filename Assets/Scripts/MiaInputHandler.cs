using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC
{
    public class MiaInputHandler : MonoBehaviour
    {
        MiaController miaController;
        MiaMovementHandler miaMovementHandler;
        float horizontal;
        float vertical;
        public Camera mainCamera;

        [HideInInspector]

        void Start()
        {
            miaController = GetComponent<MiaController>();
            miaMovementHandler = GetComponent<MiaMovementHandler>();
            miaController.Init();
            miaMovementHandler.Init(miaController, this);

        }
        private void FixedUpdate()
        {
            miaController.FixedTick();
            UpdateStatesFromInput();
            
        }

        void Update()
        {
            miaController.RegularTick();
            miaMovementHandler.Tick();
        }

        void UpdateStatesFromInput()
        {
            horizontal = Input.GetAxis(StaticVars.Horizontal);
            vertical = Input.GetAxis(StaticVars.Vertical);

            miaController.horizontal = horizontal;
            miaController.vertical = vertical;

            Vector3 h = mainCamera.transform.right * horizontal;
            Vector3 v = mainCamera.transform.forward * vertical;

            //set y to 0
            h.y = 0;
            v.y = 0;

            Vector3 moveDirection = (h + v).normalized;
            miaController.moveDirection = moveDirection;

         

            miaController.onLocomotion = miaController.anim.GetBool(StaticVars.onLocomotion);

            HandleRun();
            miaController.hasJumpInput = Input.GetButton(StaticVars.Jump);

        }

        private void HandleRun()
        {
            bool runInput = Input.GetKey(KeyCode.LeftShift);
            //if there is running input, (LeftShift), trigger run in MiaController and Set Running Animation.
            if (runInput)
            {
                miaController.isWalking = false;
                miaController.isRunning = true;
                miaController.isRunning = runInput;
                miaController.anim.SetInteger(StaticVars.specialType,
                    StaticVars.getAnimSpecialType(StaticVars.AnimSpecials.run));
            }
            //else return to walk.
            else
            {
                miaController.isWalking = true;
                miaController.isRunning = false;
            }

            //stop running if there is an object ahead.
            if (miaController.obstacleAhead)
            {
                miaController.isRunning = false;
            }

            //if mia is no longer running, trigger run to stop animation.
            if (!miaController.isRunning)
            {
                miaController.anim.SetInteger(StaticVars.specialType,
                   StaticVars.getAnimSpecialType(StaticVars.AnimSpecials.runToStop));
            }
        }
       
           
    }
}

