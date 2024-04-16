using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallVolumeController : MonoBehaviour {
    private float volume;

    private void Update() {
        volume = VolumeController.volume / 10;
        AudioListener.volume = volume;
    }
}
