using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
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
        transform.position -= Vector3.up * speed * Time.deltaTime;
	}
    
    public void Kill ()
    {
        ParticleSystemRenderer explosion = GameObject.Instantiate(enemyExplosionPrefab).GetComponent<ParticleSystemRenderer>();
        explosion.transform.position = transform.position;

        explosion.material.SetColor("_Color", GameUtil.GetColor(colour));
        explosion.material.SetColor("_TintColor", GameUtil.GetColor(colour));

        GameObject.Destroy(gameObject);
    }
}