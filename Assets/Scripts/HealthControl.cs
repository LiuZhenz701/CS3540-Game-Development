using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour {
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    
    public int playerHP = 100;
    public int enemyHP = 100;

    public int playerCurHP;
    public int enemyCurHP;

    void Start () {
        playerCurHP = playerHP;
        enemyCurHP = enemyHP;
    }

    void Update () {
        if (playerCurHP <= 0) {
            FindObjectOfType<LevelManager>().GameLost();
        }

        playerHealthBar.value = playerCurHP;
        enemyHealthBar.value = enemyCurHP;
    }

    void playerHit(int dmgAmount) {
        if (playerCurHP > 0) {
            playerCurHP = -dmgAmount;
        }        
    }

    void enemyHit(int dmgAmount) {
        if (enemyCurHP > 0) {
            enemyCurHP -= dmgAmount;
        }
    }

    void playerGainHealth(int healingAmount) {
        if (playerCurHP < playerHP) {
            playerCurHP += healingAmount;
        }
    }
}