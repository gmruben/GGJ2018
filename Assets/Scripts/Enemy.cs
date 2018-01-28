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

    public void Init(WaveColour colour, float speed)
    {
        this.colour = colour;
        this.speed = speed;

        renderer.color = GameUtil.GetColor(colour);
        radius = GetComponent<SphereCollider>().radius;

        EnemySpawner.instance.AttachKill(this);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.K)) Kill(2);
        transform.position -= Vector3.up * speed * Time.deltaTime;
	}
    
    public void Kill (int combo)
    {
        ParticleSystemRenderer explosion = GameObject.Instantiate(enemyExplosionPrefab).GetComponent<ParticleSystemRenderer>();
        explosion.transform.position = transform.position;

        explosion.material.SetColor("_Color", GameUtil.GetColor(colour));
        explosion.material.SetColor("_TintColor", GameUtil.GetColor(colour));

        CameraShake.instance.Shake(0.5f, 0.5f);
        if (OnKilled != null) OnKilled(this, combo);

        GameObject.Destroy(gameObject);
    }
}