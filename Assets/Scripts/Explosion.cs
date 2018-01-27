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

    public TowerId id;
    public WaveColour colour;

    public bool hasCollided;

    public CurvedLineRenderer linerenderer;

    public AnimationCurve alpha_curve;

    private Color color_init, color_noalpha, color_halfalpha;
    public void Init(TowerId id, WaveColour colour)
    {
        this.id = id;
        this.colour = colour;
        this.counter = 0.0F;

        color_init = GameUtil.GetColor(colour);
        color_halfalpha = color_init;
        color_halfalpha.a = 0.5F;
        color_noalpha = color_init;
        color_noalpha.a = 0.0F;
        renderer.color = color_halfalpha;

        transform.localScale = Vector3.one * GameUtil.explosionRadius * 2;

        linerenderer.line.SetColors(color_init, color_init);
    }

	void Update ()
    {
        counter += Time.deltaTime;
        Color current = Color.Lerp(color_init, color_noalpha, alpha_curve.Evaluate(counter/explosionTime));
        renderer.color = Color.Lerp(color_halfalpha, color_noalpha, alpha_curve.Evaluate(counter/explosionTime));
        linerenderer.line.SetColors(current, current);
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
            other.GetComponent<Bullet>().Kill();
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