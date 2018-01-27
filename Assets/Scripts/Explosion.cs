using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public event Action OnDead;

    public float maxScale;
    public float explosionTime;

    public SpriteRenderer renderer;

    public GameObject joinedAreaPrefab;

    private float counter;

    public PlayerId id;
    public WaveColour colour;

    public bool hasCollided;

    public CurvedLineRenderer linerenderer;

    public GameObject LinePointPrefab;
    
    public AnimationCurve influencerate;
	 float lifetime = 0.0F;

    public void Init(PlayerId id, WaveColour colour)
    {
        this.id = id;
        this.colour = colour;

        Color c = GameUtil.GetColor(colour);
        renderer.color = new Color(c.r, c.g, c.b, 0.5f);

        transform.localScale = Vector3.one * GameUtil.explosionRadius * 2;
        // GenerateLinePoints(30);

        linerenderer.line.SetColors(c, c);
    }

	void Update ()
    {
        counter += Time.deltaTime;
        if (counter >= explosionTime)
        {
            Kill();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.tag == "Explosion")
        {
            Explosion otherExplosion = other.GetComponent<Explosion>();
            if (otherExplosion.id != id)
            {
                Vector3 hv = other.transform.position - transform.position;
                Vector3 pos = transform.position + (hv * 0.5f);

                JoinedArea joinedArea = GameObject.Instantiate(joinedAreaPrefab).GetComponent<JoinedArea>();
                joinedArea.Init(this, otherExplosion);

                joinedArea.transform.position = pos;

                hasCollided = true;
                otherExplosion.hasCollided = true;
            }
        }
        else if (other.tag == "Bullet")
        {
            Bullet otherBullet = other.GetComponent<Bullet>();
            if (otherBullet.id != id)
            {
                other.GetComponent<Bullet>().Kill();
            }
        }
    }

    private void Kill ()
    {
        if (OnDead != null) OnDead();
        GameObject.Destroy(gameObject);
    }


}

public enum WaveColour
{
    Red,
    Blue,
    Yellow,

    Purple,
    Green,
    Orange,

    None
}