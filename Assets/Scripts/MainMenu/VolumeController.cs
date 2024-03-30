using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {
    public Slider volumeSlider;
    float volume;

    void Start() {
        volume = volumeSlider.value;
    }

    void Update() {
        volume = volumeSlider.value;
    }

    public float getVolume() {
        return volume;
    }
}
