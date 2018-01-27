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
    public TrailRenderer[] trailRenderers;
    
    public void Init(PlayerId id, WaveColour colour, Vector3 direction)
    {
        this.id = id;
        this.direction = direction;
        this.colour = colour;

        Color c = GameUtil.GetColor(colour);
        renderer.material.SetColor("_Color", c);

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].startColor = c;
            trailRenderers[i].endColor = new Color(c.r, c.g, c.b, 0.25f);
        }
        
        transform.rotation = Quaternion.FromToRotation (Vector3.up, direction);
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
