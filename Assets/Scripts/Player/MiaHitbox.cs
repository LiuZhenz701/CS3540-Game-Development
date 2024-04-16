using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiaHitbox : MonoBehaviour
{
    public static float blinkIntensity = 1f;
    public static float blinkDuration = .5f;
    public static float blinkTimer;
   // AIRagdoll ragdoll;
    GameObject levelManager;
    HealthControl healthControl;
    public static bool gotHitAlready = false; 
    public AudioClip miaHitSFX;

    // Start is called before the first frame update
    void Start()
    {
      //  ragdoll = GetComponent<AIRagdoll>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        healthControl = levelManager.GetComponent<HealthControl>();

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("AIHurtbox") && AIAgentAnimHandler.isAttackPlaying)
        {
            if (!gotHitAlready)
            {
                gotHitAlready = true;
                healthControl.playerHit();

                AudioSource.PlayClipAtPoint(miaHitSFX, Camera.main.transform.position, .25f);

                StartCoroutine(GetHit());
            }
        }
    }
    IEnumerator GetHit()
    {
        yield return new WaitForSeconds(1f);
        print("enemy hit player connected");
        blinkTimer = blinkDuration;
        gotHitAlready = false;
    }
}
