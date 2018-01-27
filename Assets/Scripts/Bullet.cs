using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;

    public PlayerId id;
    public WaveColour colour;
    public GameObject explosionPrefab;

    public Vector3 direction;
    public MeshRenderer renderer;

    public void Init(PlayerId id, WaveColour colour, Vector3 direction)
    {
        this.id = id;
        this.direction = direction;
        this.colour = colour;

        renderer.material.SetColor("_Color", GameUtil.GetColor (colour));
    }

    public void Explode ()
    {
        Explosion explosion = GameObject.Instantiate(explosionPrefab).GetComponent<Explosion> ();
        explosion.Init (id, colour);

        explosion.transform.position = transform.position;

        GameObject.Destroy(gameObject);
    }

    public void Kill ()
    {
        GameObject.Destroy(gameObject);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
}
