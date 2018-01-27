using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float maxPosX = 6;

    public float minSpawnTime;
    public float maxSpawnTime;

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
        WaveColour colour = colours[Random.Range(0, colours.Length)];

        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy> ();
        enemy.Init(colour);

        enemy.transform.position = transform.position;

        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        counter = 0.0f;
    }
}