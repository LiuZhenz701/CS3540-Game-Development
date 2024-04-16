using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;

public class MiaHurtbox : MonoBehaviour
{
    Rigidbody[] rbs;
    MiaController miaController;
    CapsuleCollider[] colliders;



    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            if (rb.gameObject.tag != "Player")
            {
                rb.isKinematic = true;

            }
        }
        colliders = GetComponentsInChildren<CapsuleCollider>();
        foreach (var c in colliders)
        {
            if (c.gameObject.tag != "Player")
            {
                c.radius *= 5;
                c.height *= 4;
                c.isTrigger = true;
                c.gameObject.tag = "MiaHurtbox";
            }
        }

        DeactivateColliders();
        miaController = GetComponent<MiaController>();
    }

    void Update()
    {
        //enable hurtboxes if punching or kicking
        if (miaController.isPunching || miaController.isKicking)
        {
            ActivateColliders();
        } 
        else
        {
            DeactivateColliders();
        }
    }

    public void DeactivateColliders()
    {
        foreach (var c in colliders)
        {
            if (c.gameObject.tag != "Player")
            {
                c.isTrigger = true;
                c.gameObject.tag = "MiaInactiveHurtbox";
                print("set ianctive hurtboxes");
            }
          
        }

    }

    public void ActivateColliders()
    {
        foreach (var c in colliders)
        {
            if (c.gameObject.tag != "Player")
            {
                c.isTrigger = false;
                c.gameObject.tag = "MiaHurtbox";
                print("set active hurtboxes!");
            }

        }
    }
}
