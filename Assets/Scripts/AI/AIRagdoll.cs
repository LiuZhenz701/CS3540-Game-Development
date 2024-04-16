using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator anim;
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateRagdoll()
    {
        foreach (var rb in rbs)
        {
           
            rb.isKinematic = true;
    
        }
        anim.enabled = true;

    }

    public void ActivateRagdoll()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
        anim.enabled = false;
    }
}
