using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PoisonTextChange : MonoBehaviour
{
    public Text HUDText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Invoke("ChangeTextOne", 5f);
    }

    void ChangeTextOne()
    {
        HUDText.text = "Lookout for leftover burgers to restore health!";
        Invoke("ChangeTextTwo", 8f);
    }

    void ChangeTextTwo()
    {
        HUDText.text = "Make it a combo deal with a soda to speed up!";
        Invoke("ChangeTextThree", 8f);
    }

    void ChangeTextThree()
    {
        HUDText.text = "You are ready to take on the granny army";
        Invoke("LoadLevelTwo", 3f);
    }

    void LoadLevelTwo()
    {
        SceneManager.LoadScene("Level2");
    }
}
