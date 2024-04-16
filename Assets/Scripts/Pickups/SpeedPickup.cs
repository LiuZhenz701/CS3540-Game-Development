using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    public float hoverSpeed = 1f;
    public float hoverHeight = 0.5f;
    public float rotationSpeed = 50f;
    public float respawnTimer = 5f;

    public AudioClip pickupSound;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position based on a sine wave to create a hovering effect
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Set the new position
        transform.position = new Vector3(transform.position.x, newY + 2, transform.position.z);

        // Rotate the cube around its own axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player hit speed");

            //audio clip
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            //particle effects
            

            //add speed

            gameObject.SetActive(false);
            Invoke("Respawn", respawnTimer);
        }
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }
}
