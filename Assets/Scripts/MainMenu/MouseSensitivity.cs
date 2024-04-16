using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour {
    public Slider mouseSlider;
    public static float mouseSensitivity;

    private void Start() {
        if (PlayerPrefs.HasKey("SensitivityValue")) {
            mouseSlider.value = PlayerPrefs.GetFloat("SensitivityValue");
            mouseSensitivity = PlayerPrefs.GetFloat("SensitivityValue");
        }
    }

    private void Update() {
        PlayerPrefs.SetFloat("SensitivityValue", mouseSlider.value * 100);
        mouseSensitivity = PlayerPrefs.GetFloat("SensitivityValue");
    }
}
