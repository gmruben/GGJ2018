using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public Text scoreLabel;
    public EnemySpawner enemySpawner;

    private int score;

	void Awake ()
    {
        score = 0;
        UpdateLabel ();

        enemySpawner.OnEnemyKilled += HandleOnEnemyKilled;
    }

    private void UpdateLabel ()
    {
        scoreLabel.text = string.Format("SCORE: {0}", score);
    }

    private void HandleOnEnemyKilled ()
    {
        score++;
        UpdateLabel();
    }
}