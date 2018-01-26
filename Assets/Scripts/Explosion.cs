using System.Collections;
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
    public ExplosionColour colour;

    public bool hasCollided;

    public void Init(TowerId id, ExplosionColour colour)
    {
        this.id = id;
        this.colour = colour;

        Color c = GameUtil.GetColor(colour);
        renderer.color = new Color(c.r, c.g, c.b, 0.5f);
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
        Debug.Log("TAG: " + other.tag);
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

public enum ExplosionColour
{
    Red,
    Blue
}