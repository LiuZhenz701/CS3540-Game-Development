using System.Collections;
using System.Collections.Generic;
using TPC;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour {
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    
    public int playerHP = 100;
    public int enemyHP = 100;

    public int playerCurHP;
    public int enemyCurHP;
    public int playerTakesThisDMG;
    public int enemyTakesThisDMG;

    GameObject player;
    public int damageMultiplyer = 0;


    public void Start () {
        playerCurHP = playerHP;
        enemyCurHP = enemyHP;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update () {
        if (playerCurHP <= 0) {
            print("player died");
            FindObjectOfType<LevelManager>().GameLost();
        }

        playerHealthBar.value = playerCurHP;
        enemyHealthBar.value = enemyCurHP;
    }

    public void playerHit() {
        if (playerCurHP > 0) {
            playerCurHP -= playerTakesThisDMG;
        }
        

            
        
    }

    public void playerHitPoison(int damage) {
        if (playerCurHP > 0) {
            playerCurHP -= damage;
        }
        

            
        
    }

    public void enemyHit()
    {
        if (enemyCurHP > 0)
        {
            enemyCurHP -= enemyTakesThisDMG;

        }
    }


    public void playerGainHealth(int healingAmount) {
        if (playerCurHP < playerHP) {
            playerCurHP += healingAmount;
        }
    }
}