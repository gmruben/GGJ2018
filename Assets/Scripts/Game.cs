using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public static Game instance;
    public BottomCollider bottomCollider;
    public int gameOverNumEnemies = 3;

    public GameObject gameOverScreen;

    public GameObject enemySpawner;
    public GameObject towerLeft;
    public GameObject towerRight;

    private int numEnemies = 0;
    private bool isGameOver;

    

    void Awake ()
    {
        instance = this;
        isGameOver = false;
        gameOverScreen.SetActive (false);
    }
    void OnEnable ()
    {
        bottomCollider.OnBottomReached += HandleOnBottomReached;
    }

    void OnDisable ()
    {
        bottomCollider.OnBottomReached -= HandleOnBottomReached;
    }

    void Update ()
    {
        if (isGameOver)
        {
            if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown("Fire2"))
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

    public void HandleOnBottomReached ()
    {
        numEnemies++;
        if (numEnemies >= gameOverNumEnemies)
        {
            isGameOver = true;

            enemySpawner.SetActive (false);
            towerLeft.SetActive(false);
            towerRight.SetActive(false);

            gameOverScreen.SetActive(true);
        }
    }
}