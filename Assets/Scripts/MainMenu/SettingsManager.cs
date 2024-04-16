using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    private void Start() {
        if (!PlayerPrefs.HasKey("VolumeValue")) {
            PlayerPrefs.SetFloat("VolumeValue", 10f);
        }
        if (!PlayerPrefs.HasKey("SensitivityValue")) {
            PlayerPrefs.SetFloat("SensitivityValue", 1000f);
        }
    }

    private void Update() {
        Debug.Log(PlayerPrefs.GetFloat("VolumeValue"));
        Debug.Log(PlayerPrefs.GetFloat("SensitivityValue"));
    }
}
