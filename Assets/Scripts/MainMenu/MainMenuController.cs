using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public static bool inMainMenu = true;

    public void ContinueGame() {
        string lastScene = PlayerPrefs.GetString("LastScene");

        if (!string.IsNullOrEmpty(lastScene)) {
            SceneManager.LoadScene(lastScene);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void StartNewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
