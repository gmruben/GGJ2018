using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{

    private Canvas _canvas;
    public Text scoreLabel;
    public EnemySpawner enemySpawner;



    private int score;

	void Awake ()
    {
        score = 0;
        UpdateLabel ();

        enemySpawner.OnEnemyKilled += HandleOnEnemyKilled;
        _canvas = this.GetComponent<Canvas>();
    }

    void Update () {
		
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

[System.Serializable]
public struct Overlay
{
    public string name;
    public float lifetime;

    public string WaitForInput;
}