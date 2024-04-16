using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AIHitbox : MonoBehaviour
{
    public static float blinkIntensity = 1f;
    public static float blinkDuration = .5f;
    public static float blinkTimer;
    AIRagdoll ragdoll;
    public GameObject levelManager;
    HealthControl healthControl;
    public static bool gotHitAlready = false; //to be used between all colliders on granny


    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<AIRagdoll>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        healthControl = levelManager.GetComponent<HealthControl>();
    }

 


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("MiaHurtbox")) {
            if (!gotHitAlready)
            {
           //     CameraShake.instance.ShakeCamera();
                gotHitAlready = true;
                healthControl.enemyHit();

                StartCoroutine(SloMo());
                StartCoroutine(GetHit());
            }
        }
    }

  
    IEnumerator GetHit()
    {
        yield return new WaitForSeconds(.4f);
        print("hit connected");
        AIAgentAnimHandler.gotHitTrigger = true;
        blinkTimer = blinkDuration;
        gotHitAlready = false;


    }
    IEnumerator SloMo()
    {
        yield return new WaitForSeconds(.4f);
        Time.timeScale = .5f;
        StartCoroutine(EndSloMo());
    }
    IEnumerator EndSloMo()
    {
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 1f;
    }



}
