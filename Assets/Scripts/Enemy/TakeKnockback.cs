using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;

public class TakeKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;


    public void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("collision");
            if (collision.gameObject.GetComponent<MiaController>().isPunching ||
                collision.gameObject.GetComponent<MiaController>().isKicking)
            {
                // Calculate the knockback direction from the player's position to this object's position
                Vector3 direction = transform.position - collision.gameObject.transform.position;
                ApplyKnockback(direction);
            }
            
        }

    }
    // Function to apply knockback force
    public void ApplyKnockback(Vector3 direction)
    {
        // Apply knockback force in the specified direction
        GetComponent<Rigidbody>().AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
    }
}
