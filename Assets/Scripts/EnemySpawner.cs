using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event Action OnEnemyKilled;

    public GameObject[] enemyPrefabs;
    public float maxPosX = 6;

    public float minSpawnTime;
    public float maxSpawnTime;

    public float minEnemySpeed;
    public float maxEnemySpeed;

    public WaveColour[] colours;
    public float speed;

    private float counter;
    private float spawnTime;
    private int direction = 1;

    void Awake ()
    {
        Spawn();
    }

    void Update ()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;
        if (Mathf.Abs (transform.position.x) > maxPosX)
        {
            transform.position = new Vector3(maxPosX * direction, transform.position.y, transform.position.z);
            direction = -direction;
        }

        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            Spawn();
        }
    }

    private void Spawn ()
    {
        WaveColour colour = colours[UnityEngine.Random.Range(0, colours.Length)];
        float speed = UnityEngine.Random.Range(minEnemySpeed, maxEnemySpeed);

        Enemy enemy = GameObject.Instantiate(enemyPrefabs[UnityEngine.Random.Range (0, enemyPrefabs.Length)]).GetComponent<Enemy> ();
        enemy.Init(colour, speed);

        enemy.transform.position = transform.position;
        enemy.OnKilled += HandleOnEnemyKilled;

        spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        counter = 0.0f;
    }

    private void HandleOnEnemyKilled (Enemy enemy)
    {
        enemy.OnKilled -= HandleOnEnemyKilled;
        if (OnEnemyKilled != null) OnEnemyKilled ();
    }
}