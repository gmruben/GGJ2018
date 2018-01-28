using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public List<WaveGroupData> groups;
    public float DelayBetweenMoves = 0.0F, DelayBetweenGroups = 0.0F;
    private int current = 0;

    public string OverlayMessage;

    public float SpawnEnemyRateMin = 0.0F, SpawnEnemyRateMax = 0.0F;

    public bool complete = false;
    GameObject grouptarg;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartWave()
    {
        current = 0;
        StartCoroutine(StartWaveRoutine());
    }
    IEnumerator StartWaveRoutine()
    {
        complete = false;
        if (OverlayMessage != string.Empty) OverlayHUD.instance.LoadOverlay(OverlayMessage);

        while (current < groups.Count)
        {
            grouptarg = Instantiate(groups[current].GroupPrefab);
            grouptarg.transform.position = this.transform.position + Vector3.left * Random.Range(-GameUtil.ScreenXRange, GameUtil.ScreenXRange);
            List<Enemy> spawnedenemies = new List<Enemy>();
            foreach (Enemy e in grouptarg.GetComponentsInChildren<Enemy>())
            {
                WaveColour colour = (e.colour != WaveColour.None) ? e.colour : GameUtil.RandomColor;//colours[UnityEngine.Random.Range(0, colours.Length)];
                e.Init(colour, 0.0F);
                spawnedenemies.Add(e);
            }

            if (groups[current].Speed == 0.0F) groups[current].Speed = GameUtil.RandomSpeed;
            Vector3 lastpoint = grouptarg.transform.position;

            for (int i = 0; i < groups[current].wavepoints.Length; i++)
            {
                Transform p = groups[current].wavepoints[i];
                while (Vector3.Distance(grouptarg.transform.position, p.position) > 0.2F)
                {
                    Vector3 vel = p.position - grouptarg.transform.position;

                    grouptarg.transform.position += vel.normalized * Time.deltaTime * groups[current].Speed;
                    yield return null;
                }


                lastpoint = grouptarg.transform.position;
                if (DelayBetweenMoves > 0.0F) yield return new WaitForSeconds(DelayBetweenMoves);
            }

            if (groups[current].MoveToTownOnExit)
            {

                while (Vector3.Distance(grouptarg.transform.position, Game.instance.bottomCollider.transform.position) > 0.2F)
                {
                    Vector3 vel = Game.instance.bottomCollider.transform.position - grouptarg.transform.position;

                    grouptarg.transform.position += vel.normalized * Time.deltaTime * groups[current].Speed;
                    yield return null;
                }
            }
            else if(groups[current].WaitForAllDestroyed)
            {
                bool enemiesalive = true;
                while(enemiesalive)
                {
                    enemiesalive = false;
                    for(int i = 0; i < spawnedenemies.Count; i++) if(spawnedenemies[i] != null) 
                    {
                        enemiesalive = true;
                        break;
                    }
                    yield return null;
                }
            }

            Destroy(grouptarg);
            current++;
        }
        complete = true;
        End();
    }

    public void End()
    {
        if(grouptarg != null) Destroy(grouptarg);
        if(OverlayMessage != string.Empty) OverlayHUD.instance.EndOverlay();
        StopAllCoroutines();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(this.transform.position, 0.2F);
    }

}

[System.Serializable]
public class WaveGroupData
{
    public Transform[] wavepoints;
    public bool MoveToTownOnExit, WaitForAllDestroyed;
    public GameObject GroupPrefab;
    public float DelayedEntrance;

    public float Speed = 10.0F;

}

