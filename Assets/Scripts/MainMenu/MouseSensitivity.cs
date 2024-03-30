using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour {
    public Slider mouseSlider;
    float mouseSensitivity;

    void Start() {
        mouseSensitivity = mouseSlider.value;
    }

    void Update() {
        mouseSensitivity = mouseSlider.value;
    }
    
    public float getMouseSensitivity() {
        return mouseSensitivity;
    }
}
