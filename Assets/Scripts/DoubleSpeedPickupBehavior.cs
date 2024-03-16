using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class DoubleSpeedPickupBehavior : MonoBehaviour {
    public GameObject player;

    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update() {
        gameObject.transform.Rotate(0, 0, 30 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (player != null) {
            DoubleSpeed(player);
            OnDestroy();
        }
    }

    private void OnDestroy() {
        // TODO: pickup destroy sound

        // destroy the pickup after 1 second
        Destroy(gameObject, 1);
    }

    private void DoubleSpeed(GameObject player) {
        player.GetComponent<CharacterControl>().DoubleSpeed(5f);
    }
}