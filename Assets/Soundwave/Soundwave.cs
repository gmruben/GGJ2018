using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundwave : MonoBehaviour
{
    public CurvedLineRenderer linerenderer;

    public GameObject LinePointPrefab;

    public List<WavePoint> linepoints;

    public AnimationCurve influencerate;
	 float lifetime = 0.0F;

    // Use this for initialization
    void Start()
    {           
		GenerateLinePoints(30);
    }

    // Update is called once per frame
    void Update()
    {
		if(lifetime > 1.0F) return;
			lifetime += Time.deltaTime;
            if (linepoints != null && linepoints.Count > 0)
            {
                for (int i = 0; i < linepoints.Count; i += 2)
                {
                    linepoints[i].rate = Mathf.PingPong(Time.time * 10, 1.0F);
                    linepoints[i].velocity = Vector3.Lerp(linepoints[i].vel_min, linepoints[i].vel_max, linepoints[i].rate);
                    linepoints[i].currentpoint = linepoints[i].initialpoint + (linepoints[i].velocity * influencerate.Evaluate(lifetime));
                    foreach (Transform t in linepoints[i].transform)
                    {
                        t.position = linepoints[i].currentpoint;
                    }

                }
			}
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
        for (int i = 0; i <= num; i++)
        {
            GameObject linepoint = Instantiate(LinePointPrefab);
            linepoint.name = "Line point " + i;

            float angle = 360 / num * i;

            linepoint.transform.position = RandomCircle(this.transform.position, GameUtil.explosionRadius, angle);
            linepoint.transform.SetParent(this.transform);

            Vector3 velc = linepoint.transform.position - this.transform.position;
            velc /= 10;
            WavePoint c = new WavePoint(linepoint.transform, velc, -velc);
            linepoints.Add(c);
        }
        GameObject linepointfinal = Instantiate(LinePointPrefab);
        linepointfinal.name = "Line point " + num;
        linepointfinal.transform.position = RandomCircle(this.transform.position, GameUtil.explosionRadius, 0);
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
