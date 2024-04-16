using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckNewGame : MonoBehaviour {

    void Update() {
        string lastScene = PlayerPrefs.GetString("LastScene");

        if (string.IsNullOrEmpty(lastScene)) {
            gameObject.GetComponent<Button>().interactable = false;
        } else {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
