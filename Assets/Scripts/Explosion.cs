﻿using System.Collections;
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

    public GameObject LinePointPrefab;

    public List<WavePoint> linepoints;
    public float radius = 25;

    public AnimationCurve influencerate;
	 float lifetime = 0.0F;

    public void Init(TowerId id, WaveColour colour)
    {
        this.id = id;
        this.colour = colour;

        Color c = GameUtil.GetColor(colour);
        renderer.color = new Color(c.r, c.g, c.b, 0.5f);

        transform.localScale = Vector3.one * GameUtil.explosionRadius * 2;
         GenerateLinePoints(30);

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
            other.GetComponent<Bullet>().Kill();
        }
    }

    private void Kill ()
    {
        if (OnDead != null) OnDead();
        GameObject.Destroy(gameObject);
    }


    public void GenerateLinePoints(int num)
    {
        if (linepoints != null && linepoints.Count > 0)
        {
            for (int i = 0; i < linepoints.Count; i++)
            {
				if(linepoints[i] == null || linepoints[i].transform == null) continue;
                for (int t = 0; t < linepoints[i].transform.Count; i++)
                {
                    GameObject.Destroy(linepoints[i].transform[t].gameObject);
                }

            }
            linepoints.Clear();
        }
        linepoints = new List<WavePoint>();
        for (int i = 0; i < num; i++)
        {
            GameObject linepoint = Instantiate(LinePointPrefab);
            linepoint.name = "Line point " + i;

            float angle = 360 / num * i;

            linepoint.transform.position = RandomCircle(this.transform.position, radius, angle);
            linepoint.transform.SetParent(this.transform);

            Vector3 velc = linepoint.transform.position - this.transform.position;
            velc /= 10;
            WavePoint c = new WavePoint(linepoint.transform, velc, -velc);
            linepoints.Add(c);
        }
        GameObject linepointfinal = Instantiate(LinePointPrefab);
        linepointfinal.name = "Line point " + num;
        linepointfinal.transform.position = RandomCircle(this.transform.position, radius, 0);
        Vector3 vel = linepointfinal.transform.position - this.transform.position;
        vel /= 10;
        linepoints[0].transform.Add(linepointfinal.transform);
        linepointfinal.transform.SetParent(this.transform);
    }

    Vector3 RandomCircle(Vector3 center, float radius, float a)
    {
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public class WavePoint
    {
        public List<Transform> transform;
        public Vector3 initialpoint;
        public Vector3 currentpoint;

        public Vector3 velocity;
        public Vector3 vel_min, vel_max;

        public float rate;
        public WavePoint(Transform t, Vector3 min, Vector3 max)
        {
            transform = new List<Transform>();
            transform.Add(t);
            initialpoint = t.position;
            currentpoint = t.position;
            vel_min = min;
            vel_max = max;
            velocity = Vector3.zero;
            rate = 0.0F;
        }
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