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

    GameObject player;


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

    public void playerHit(int dmgAmount) {
        print(dmgAmount);
        Debug.Log("before: " + playerCurHP);
        if (playerCurHP > 0) {
            playerCurHP -= dmgAmount;
        }
        
        Debug.Log("after: " + playerCurHP);

       // AudioSource.PlayClipAtPoint(playerHitSFX, Camera.main.transform.position);
            
        
    }

    public void enemyHit(int dmgAmount)
    {
        if (enemyCurHP > 0)
        {
            enemyCurHP -= dmgAmount;
      /*      
            if (player.GetComponent<MiaController>().isPunching)
            {
                AudioSource.PlayClipAtPoint(punchConnectedSFX, Camera.main.transform.position, 0.33f);
            }
            else if (player.GetComponent<MiaController>().isKicking)
            {
                AudioSource.PlayClipAtPoint(kickConnectedSFX, Camera.main.transform.position, 0.33f);
            }
            */
        }
    }


    public void playerGainHealth(int healingAmount) {
        if (playerCurHP < playerHP) {
            playerCurHP += healingAmount;
        }
    }
}