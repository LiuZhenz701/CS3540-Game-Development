using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
   void Update() {
        gameObject.transform.Rotate(0, 1 * Time.deltaTime, 0);
    }
}
