using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime;

    private float counter;

    void Awake ()
    {
        GameObject enemy = GameObject.Instantiate(enemyPrefab);
        enemy.transform.position = transform.position;

    }

    void Update ()
    {
        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.position = transform.position;

            counter = 0;
        }
    }
}