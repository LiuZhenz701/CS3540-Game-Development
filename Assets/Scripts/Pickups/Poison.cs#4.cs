using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public int damageAmount;
    public float damageSpeed = 5f;
    public GameObject levelManager;
    bool isInside;
    HealthControl healthControl;



    // Start is called before the first frame update
    void Start()
    {
        isInside = false;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        healthControl = levelManager.GetComponent<HealthControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Coroutine to repeatedly apply damage
    IEnumerator ApplyDamage()
    {
        while (isInside)
        {
            healthControl.playerHitPoison(damageAmount); // Call your damage method here
            yield return new WaitForSeconds(damageSpeed);
        }
    }

    // Trigger when player enters the poison area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            StartCoroutine(ApplyDamage());
        }
    }

    // Trigger when player exits the poison area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    }
    
}
