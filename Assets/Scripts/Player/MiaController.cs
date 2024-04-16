using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace TPC
{
    public class MiaController : MonoBehaviour
    {
        [Header("Info")]
        public GameObject miaPrefab;
        public bool inGame;

        [Header("Stats")]
        public float groundDistance = 0.6f;
        public float groundOffset = 0;
        public float distanceToCheckForwards = 1.3f;
        public float walkSpeed = 2f;
        public float sprintSpeed = 7f;
        public float jumpForce = 10f;
        public float gravity = 10f;
        public float airControl = 5f;
        public float airtimeThreshold;
        public float padForce = 10f;
        [Header("Inputs")]
        public float horizontal;
        public float vertical;

        [Header("States")]
        public bool obstacleAhead;
        public bool groundForward;
        public float groundAngle;

        #region StateRequests
        [Header("State Requests")]
        public CharStates curState;
        public bool onGround;
        public bool isWalking;
        public bool isRunning;
        public bool onLocomotion;
        public bool inAngleOfMoveDirection;
        public bool hasJumpInput;
        public bool canJump;
        public bool isJumping;
        public bool hasPunchInput;
        public bool hasKickInput;
        public bool isPunching;
        public bool isKicking;
        public bool isOnJumpPad;
        private bool hasPlayedAttackSFX = false;
        private Coroutine speedBoostCoroutine;


        #endregion

        #region References
        [Header("References")]
        GameObject activeModel;
        public Animator anim;
        public CharacterController controller;
        public Camera mainCamera;
        public CinemachineFreeLook freeLookCamera;
        SkinnedMeshRenderer[] skinnedMeshRenderers;
        public AudioClip missedAttackSFX;
        public GameObject punchVFX;
        public GameObject kickVFX;
        public GameObject lightningVFX;

        #endregion

        #region Variables
        [HideInInspector]
        public Vector3 moveDirection;
        [HideInInspector]
        public Vector3 aimPosition;
        public float airTime;
        [HideInInspector]
        public bool prevGround;
        #endregion

        public enum CharStates
        {
            idle, moving, inAir, hold, inCombo
        }

        #region Init Game
        public void Init()
        {
            inGame = true;
            //CreateModel();
            controller = GetComponent<CharacterController>();
            canJump = true;
            //    gameObject.layer = 8;
            //     ignoreLayers
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            
        }
        void CreateModel()
        {
            //activeModel = Instantiate(miaPrefab) as GameObject;
            activeModel.transform.localPosition = Vector3.zero;
            activeModel.transform.localEulerAngles = Vector3.zero;
            activeModel.transform.localScale = Vector3.one;
        }
        #endregion

        public void Start()
        {
          //  punchVFX = GameObject.FindWithTag("MiaLeftPunchVFX");
           // kickVFX = GameObject.FindWithTag("MiaRightKickVFX");
        }
        public void FixedTick()
        {
            obstacleAhead = false;
            groundForward = false;
            onGround = controller.isGrounded;

            if (onGround)
            {
                anim.SetBool(StaticVars.inAir, false);
                Vector3 origin = transform.position;

                //check if there are obstacles ahead:
                origin += Vector3.up * .75f;
                IsClear(origin, transform.forward, distanceToCheckForwards, ref obstacleAhead);
                if(!obstacleAhead)
                {
                    //check if there is ground ahead:
                    origin += transform.forward * .6f;
                    IsClear(origin, -Vector3.up, groundDistance * 3, ref groundForward);
                }
                else
                {
                    obstacleAhead = false;
                }

            } else
            {
                AirTime();

            }

            UpdateState();

        }
        public void RegularTick()
        {
            UpdateMouseSensitivity();
            onGround = controller.isGrounded;
            BlinkWhenHitEffect();
            if (Time.timeScale == .5f)
            {
                lightningVFX.SetActive(true);
                // Trigger the shake effect
                CinemachineBasicMultiChannelPerlin noise = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (noise != null)
                {
                    noise.m_AmplitudeGain = 1.5f; // Adjust amplitude for intensity
                    noise.m_FrequencyGain = 2f; // Adjust frequency for speed
                }
            }
            else
            {
                lightningVFX.SetActive(false);

                // Stop the shake effect
                CinemachineBasicMultiChannelPerlin noise = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (noise != null)
                {
                    noise.m_AmplitudeGain = 0f;
                }

            }


        }

        void UpdateState()
        {
            if (curState == CharStates.hold)
            {
                return;
            }

            if (horizontal != 0 && vertical != 0)
            {
                curState = CharStates.moving;
            }
            if (isPunching || isKicking)
            {
               
                curState = CharStates.inCombo;
            }
            else
            {
                curState = CharStates.idle;
            }

            if (!onGround)
            {
                curState = CharStates.inAir;
            }
        }

   

        void IsClear(Vector3 origin, Vector3 direction, float distance, ref bool isHit)
        {
            RaycastHit hit;
            Debug.DrawRay(origin, direction * distance, Color.green);
            if (Physics.Raycast(origin, direction, out hit, distance))
            {
                isHit = true;
            }
            else { isHit = false; } 

           
        }

        void AirTime()
        {
            if(!isJumping)
            {
                //anim.SetBool(StaticVars.inAir, !onGround);
            }
            //landing from the air to the ground
            if (onGround)
            {
                if (prevGround != onGround)
                {
                    anim.SetInteger(StaticVars.jumpType, (airTime > airtimeThreshold) ? (horizontal != 0 || vertical != 0) ? 2 : 1 : 0);
                }
                airTime = 0;
               // print(onGround);

            }
            //add air time if still in air
            else
            {
                airTime += Time.deltaTime;
            }
            prevGround = onGround;
        }

        //mirror jump animation based on which leg is ahead of the other.
        public void LegFront()
        {
            Vector3 leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            Vector3 rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
            Vector3 relativeLeftFoot = transform.InverseTransformPoint(leftFoot);
            Vector3 relativeRightFoot = transform.InverseTransformPoint(rightFoot);
           
            anim.SetBool(StaticVars.mirrorJump, relativeLeftFoot.z > relativeRightFoot.z);

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SpeedPickup"))
            {
                Debug.Log("Speed pickup collected");

                // Start the speed boost coroutine if it's not already running
                if (speedBoostCoroutine == null)
                {
                    speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine());
                }
                else
                {
                    // Restart the coroutine if the speed boost is already active
                    StopCoroutine(speedBoostCoroutine);
                    speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine());
                }
            }
            else if (other.CompareTag("BouncePad"))
            {
                Debug.Log("Bounce Pad hit");

                // Check if the character controller is not null and the player is grounded
                if (controller != null && controller.isGrounded)
                {
                    // Apply vertical force to make the character jump
                    Vector3 jumpVelocity = Vector3.up * jumpForce;
                    controller.Move(jumpVelocity * Time.deltaTime * padForce / 2);

                    // Set the isJumping flag to true
                    isJumping = true;
                    anim.SetBool("inAir", true);

                }
            }
        }

        private IEnumerator SpeedBoostCoroutine()
        {
            Debug.Log("Speed boost activated");

            // Apply the speed boost here
            float originalWalkSpeed = walkSpeed;
            walkSpeed *= 1.2f; // Double the walk speed temporarily
            float originalSprintSpeed = sprintSpeed;
            sprintSpeed *= 1.2f;
            // Wait for the specified duration (10 seconds in this case)
            yield return new WaitForSeconds(5f);

            // Revert the speed back to its original value after the duration
            walkSpeed = originalWalkSpeed;

            // Reset the coroutine reference
            speedBoostCoroutine = null;

            Debug.Log("Speed boost deactivated");
        }

        private void BlinkWhenHitEffect()
        {
            //glow when hit effect
            MiaHitbox.blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(MiaHitbox.blinkTimer / MiaHitbox.blinkDuration);
            float intensity = lerp * MiaHitbox.blinkIntensity + 1f;

            foreach (var smr in skinnedMeshRenderers)
            {
               
                
                smr.material.color = Color.white * intensity;

                
            }
        }

        private void UpdateMouseSensitivity()
        {
            print(MouseSensitivity.mouseSensitivity);
            freeLookCamera.m_XAxis.m_MaxSpeed = MouseSensitivity.mouseSensitivity;
        }
    }

}
