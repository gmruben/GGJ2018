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

    public ParticleSystemRenderer destroyParticleSystem;
    public Collider collider;
    public bool hasDesintegrated { get; private set; }

    void Awake ()
    {
        destroyParticleSystem.gameObject.SetActive(false);
    }

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

        destroyParticleSystem.material.SetColor("_Color", GameUtil.GetColor(colour));
        destroyParticleSystem.material.SetColor("_TintColor", GameUtil.GetColor(colour));

        transform.rotation = Quaternion.FromToRotation (Vector3.up, direction);
    }

    public void Explode ()
    {
        Explosion explosion = GameObject.Instantiate(explosionPrefab).GetComponent<Explosion> ();
        explosion.Init (id, colour);

        explosion.transform.position = transform.position;

        Kill();
    }

    public void Kill ()
    {
        GameObject.Destroy(gameObject);
    }

    public void Desintegrate ()
    {
        hasDesintegrated = true;

        renderer.gameObject.SetActive (false);
        for (int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].gameObject.SetActive (false);
        }

        destroyParticleSystem.gameObject.SetActive(true);
        collider.enabled = false;

        StartCoroutine(DestroyCoroutine(2.0f));
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private IEnumerator DestroyCoroutine (float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
