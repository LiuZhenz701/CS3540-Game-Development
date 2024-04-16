using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;

public class TakeKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;




    // Function to apply knockback force
    public void ApplyKnockback(GameObject other)
    {

        StartCoroutine(ApplyKnockbackEnemy(other));
        
    }


    IEnumerator ApplyKnockbackEnemy(GameObject other)
    {
        yield return new WaitForSeconds(.5f);
        ApplyKnockbackEnemyEnd(other);
    }

    public void ApplyKnockbackEnemyEnd(GameObject other)
    {
        Vector3 direction = transform.position - other.transform.position;
        // Apply knockback force in the specified direction
        GetComponent<Rigidbody>().AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
    }
}
