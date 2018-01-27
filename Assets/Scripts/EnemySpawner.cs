using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    void Awake(){instance = this;}
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

    public List<EnemyWave> randomwaves;
    public List<EnemyWave> tutewaves;

    private bool spawnEnemies;
    private int wavenum = 0;
    public EnemyWave targetwave;
   /* void Awake()
    {
        //Spawn();
    }*/

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.U))
        {
            StartWave(randomwaves[0]);
            tutewave_curr++;
        }*/
        /*transform.position += Vector3.right * direction * speed * Time.deltaTime;
        if (Mathf.Abs (transform.position.x) > maxPosX)
        {
            transform.position = new Vector3(maxPosX * direction, transform.position.y, transform.position.z);
            direction = -direction;
        }
*/

        if(targetwave != null)
        {
            if(targetwave.complete)
            {
                Debug.Log("changing targetwave");
                 wavenum ++;
                if(wavenum < tutewaves.Count)
                {
                    targetwave = tutewaves[wavenum];
                    StartWave(targetwave);
                }
                else
                {
                    GetRandomWave();
                }
            }
        }
        if (spawnEnemies)
        {
            counter += Time.deltaTime;
            if (counter >= spawnTime)
            {
                Spawn();
            }
        }

    }

    public void GetRandomWave()
    {
        targetwave = randomwaves[UnityEngine.Random.Range(0, randomwaves.Count)];
        StartWave(targetwave);
    }

    public void StartTute()
    {
        wavenum = 0;
        targetwave = tutewaves[wavenum];
        StartWave(targetwave);
    }
    

    public void StartWave(EnemyWave w)
    {
        w.StartWave();
        if (w.SpawnEnemyRateMin > 0.0F)
        {
            spawnEnemies = true;
            spawnTime = UnityEngine.Random.Range(w.SpawnEnemyRateMin, w.SpawnEnemyRateMax);
            counter = 0.0F;
            //for(int i = 0; i < w.SpawnRandomEnemies; i++) Spawn();
        }
    }
    

    private void Spawn()
    {
        WaveColour colour = colours[UnityEngine.Random.Range(0, colours.Length)];
        float speed = UnityEngine.Random.Range(minEnemySpeed, maxEnemySpeed);

        Enemy enemy = GameObject.Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)]).GetComponent<Enemy>();
        enemy.Init(colour, speed);

        enemy.transform.position = transform.position;
        enemy.OnKilled += HandleOnEnemyKilled;

        //spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        counter = 0.0f;

        Vector3 randpoint = transform.position;
        randpoint.x = UnityEngine.Random.Range(-maxPosX, maxPosX);
        transform.position = randpoint;
    }

    private void HandleOnEnemyKilled(Enemy enemy)
    {
        enemy.OnKilled -= HandleOnEnemyKilled;
        if (OnEnemyKilled != null) OnEnemyKilled();
    }
}