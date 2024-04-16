using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public int damageAmount = 2;
    public float damageSpeed = 5f;
    public LevelManager LevelManager;
    bool isHit;

    HealthControl healthControl;



    // Start is called before the first frame update
    void Start()
    {
        healthControl = LevelManager.GetComponent<HealthControl>();
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EndAttackState(float duration)
        {
            yield return new WaitForSeconds(duration);

            isHit = false;
        }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            if(!isHit)
            {
                isHit = true;
                Debug.Log("before");
                healthControl.playerHit(damageAmount);
                Debug.Log("after");
                StartCoroutine(EndAttackState(damageSpeed));
            }
        }
    }
    
}
