using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour {

    public static bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject gameHUD;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gamePaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameHUD.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame() {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameHUD.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMainMenu() {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
