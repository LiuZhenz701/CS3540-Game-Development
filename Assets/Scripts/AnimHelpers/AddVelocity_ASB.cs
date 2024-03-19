using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC
{
    //using functionality from "AdvancedThirdPersonController" By: Sharp Accent on YouTube.
    public class AddVelocity_ASB : StateMachineBehaviour
    {
        public float life = 0.4f;
        public float force = 6;
        public Vector3 direction;
        [Space]
        [Header("Overrides direction")]
        public bool useTransformForward;
        public bool additive;
        public bool onEnter;
        public bool onExit;
        [Header("On ending apply velocity, not anim state")]
        public bool onEndClampVeloctiy;

        MiaController miaController;
        MiaMovementHandler miaMovementHandler;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnter)
            {
                if (useTransformForward && !additive)
                {
                    direction = animator.transform.forward;
                }
                if (useTransformForward && additive)
                {
                    direction += animator.transform.forward;
                }
                if (miaController == null)
                {
                    miaController = animator.transform.GetComponent<MiaController>();
                }

                if (miaMovementHandler == null)
                {
                    animator.transform.GetComponent<MiaMovementHandler>();
                }

                //do something to make the character move forward here to compensate for weird anims
          //      ply.AddVelocity(direction, life, force, onEndClampVeloctiy);
            }

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onExit)
            {
                if (useTransformForward && !additive)
                {
                    direction = animator.transform.forward;
                }
                if (useTransformForward && additive)
                {
                    direction += animator.transform.forward;
                }
                if (miaController == null)
                {
                    miaController = animator.transform.GetComponent<MiaController>();
                }

                if (miaMovementHandler == null)
                {
                    animator.transform.GetComponent<MiaMovementHandler>();
                }

                //do something to make the character move forward here to compensate for weird anims
                //      ply.AddVelocity(direction, life, force, onEndClampVeloctiy);
            }

        }

    }

}
