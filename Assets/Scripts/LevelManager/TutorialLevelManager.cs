using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelManager : MonoBehaviour
{
    public float timer = 28f;
    public string nextLevel;

    private void Update()
    {

        
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
            
        else
        {
            SceneManager.LoadScene(nextLevel);
        }

        
    }
}
