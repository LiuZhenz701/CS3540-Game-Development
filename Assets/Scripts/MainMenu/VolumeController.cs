using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {
    public Slider volumeSlider;
    public static float volume;

    private void Start() {
        if (PlayerPrefs.HasKey("VolumeValue")) {
            volumeSlider.value = PlayerPrefs.GetFloat("VolumeValue");
            volume = PlayerPrefs.GetFloat("VolumeValue");
        }
    }

    private void Update() {
        PlayerPrefs.SetFloat("VolumeValue", volumeSlider.value);
        volume = PlayerPrefs.GetFloat("VolumeValue");
    }
}
