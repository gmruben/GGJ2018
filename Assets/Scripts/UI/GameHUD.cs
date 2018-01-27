using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public GameObject comboLabelPrefab;
    public Camera uiCamera;

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

    private void UpdateLabel ()
    {
        scoreLabel.text = string.Format("SCORE: {0}", score);
    }

    private void HandleOnEnemyKilled (Enemy enemy, int combo)
    {
        score = score + 1 + ((combo - 1) * 5);
        UpdateLabel();

        if (combo > 1)
        {
            ComboFader comboFader = GameObject.Instantiate(comboLabelPrefab).GetComponent<ComboFader>();

            comboFader.transform.SetParent(transform);
            comboFader.Init(combo);

            Vector3 screenViewport = uiCamera.WorldToViewportPoint(enemy.transform.position);
            Vector3 screenPosition = new Vector3(screenViewport.x * 1920 - 960, screenViewport.y * 1080 - 540, 0);

            comboFader.transform.localPosition = screenPosition;
        }
    }
}

[System.Serializable]
public struct Overlay
{
    public string name;
    public float lifetime;
}