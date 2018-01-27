using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnKilled;

    public GameObject enemyExplosionPrefab;

    public float speed;
    public MeshRenderer renderer;
    public WaveColour colour;

    public void Init(WaveColour colour)
    {
        this.colour = colour;

        renderer.material.SetColor("_Color", GameUtil.GetColor(colour));
    }

    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.K)) Kill();
        transform.position -= Vector3.up * speed * Time.deltaTime;
	}
    
    public void Kill ()
    {
        ParticleSystemRenderer explosion = GameObject.Instantiate(enemyExplosionPrefab).GetComponent<ParticleSystemRenderer>();
        explosion.transform.position = transform.position;

        explosion.material.SetColor("_Color", GameUtil.GetColor(colour));
        explosion.material.SetColor("_TintColor", GameUtil.GetColor(colour));

        CameraShake.instance.Shake(0.5f);
        if (OnKilled != null) OnKilled(this);

        GameObject.Destroy(gameObject);
    }
}