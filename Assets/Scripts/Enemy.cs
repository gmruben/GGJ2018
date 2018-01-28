using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{
    public event Action<Enemy, int> OnKilled;

    public GameObject enemyExplosionPrefab;

    private float speed;
    public SpriteRenderer renderer;
    public WaveColour colour;

    public float radius { get; private set; }

    public bool zigzag = false;
    private float zigdepth, zigtime, timecurrent;
    private Vector3 zig_nextpoint;
    private bool zigside = false;

    public void Init(WaveColour colour, float speed)
    {
        this.colour = colour;
        this.speed = speed;

        renderer.color = GameUtil.GetColor(colour);
        radius = GetComponent<SphereCollider>().radius;

        EnemySpawner.instance.AttachKill(this);

        if(zigzag)
        {
            zigdepth = UnityEngine.Random.Range(-GameUtil.ScreenXRange, GameUtil.ScreenXRange);
            zigtime = UnityEngine.Random.Range(1.5F, 3.0F);
            zig_nextpoint = transform.position + Vector3.down + (Vector3.left * zigdepth * 2 * (zigside ? 1: -1));
            timecurrent = zigtime;
        }
    }

    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.K)) Kill(2);
        if(!zigzag) transform.position -= Vector3.up * speed * Time.deltaTime;
        else
        {
            if(Vector3.Distance(transform.position, zig_nextpoint) > 0.2F)
            {
                Vector3 vel = zig_nextpoint - transform.position;
                transform.position += vel * speed * Time.deltaTime;
            }
            else if(timecurrent <= 0.0F)
            {
                zig_nextpoint = transform.position + Vector3.down + (Vector3.left * zigdepth * 2 * (zigside ? 1: -1));
                zigside = !zigside;
                timecurrent = zigtime;
            }
            else timecurrent -= Time.deltaTime;
        }
	}
    
    public void Kill (int combo)
    {
        AudioManager.PlaySFX("EnemyDies");

        ParticleSystemRenderer explosion = GameObject.Instantiate(enemyExplosionPrefab).GetComponent<ParticleSystemRenderer>();
        explosion.transform.position = transform.position;

        explosion.material.SetColor("_Color", GameUtil.GetColor(colour));
        explosion.material.SetColor("_TintColor", GameUtil.GetColor(colour));

        CameraShake.instance.Shake(0.5f, 0.5f);
        if (OnKilled != null) OnKilled(this, combo);

        GameObject.Destroy(gameObject);
    }
}