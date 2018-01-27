using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime;
    public WaveColour[] colours;

    private float counter;

    void Awake ()
    {
        Spawn();
    }

    void Update ()
    {
        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            Spawn();
            counter = 0;
        }
    }

    private void Spawn ()
    {
        WaveColour colour = colours[Random.Range(0, colours.Length)];

        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy> ();
        enemy.Init(colour);

        enemy.transform.position = transform.position;
    }
}