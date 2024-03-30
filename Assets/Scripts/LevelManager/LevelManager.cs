using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public Text timerText;
    public Text hitCountText;
    public Text gameWinText;
    public Text gameLostText;

    public float gameDuration;
    public int gameWinHits;

    public string nextLevel;

    public static bool isGameWin;
    private bool gameLost = false;

    private float timer;
    private float hitCounter;

    public AudioClip gameWinSFX;
    public AudioClip gameLoseSFX;
    public AudioClip backgroundMusic;
    bool audioClipPlayed = false;

    void Start () {
        isGameWin = false;
        timer = gameDuration;
        hitCounter = 0;

    }

    private void Update() {
        
        if (!isGameWin) {
            if (timer > 0) {
                timer -= Time.deltaTime;
                    
            }
            if (timer <= 0) {
                timer = 0;
                GameLost();
            }
        }
        if (!gameLost) {
            SetTimerText();
        }
        SetHitText();
    }

    private void SetTimerText() {
        timerText.text = Mathf.FloorToInt(timer).ToString();
    }

    private void SetHitText() {
        hitCountText.text = hitCounter.ToString();
    }

    public void dummyHit() {
        hitCounter += 1;
    }

    public void GameLost() {
        isGameWin = false;
        gameLost = true;
        gameLostText.gameObject.SetActive(true);
        if (!audioClipPlayed) 
        {
            AudioSource.PlayClipAtPoint(gameLoseSFX, Camera.main.transform.position);
            audioClipPlayed = true;
        }



        Invoke("LoadCurrentLevel", 3);
    }

    private void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin() {
        isGameWin = true;
        gameWinText.gameObject.SetActive(true);
        if (!audioClipPlayed)
        {
            AudioSource.PlayClipAtPoint(gameWinSFX, Camera.main.transform.position);
            audioClipPlayed = true;
        }

        if (!string.IsNullOrEmpty(nextLevel)) {
            Invoke("LoadNextLevel", 3);
        }
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }
}