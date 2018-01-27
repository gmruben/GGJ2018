using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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

    public AnimationCurve alpha_curve;

    private Color color_init, color_noalpha, color_halfalpha;
    public GameObject LinePointPrefab, LinePrefab;

    public AnimationCurve influencerate;
    private float lifetime = 0.0F;

    public void Init(PlayerId id, WaveColour colour)
    {
        this.id = id;
        this.colour = colour;

        counter = 0.0F;

        color_init = GameUtil.GetColor(colour);
        color_halfalpha = color_init;
        color_halfalpha.a = 0.5F;
        color_noalpha = color_init;
        color_noalpha.a = 0.0F;
        renderer.color = color_halfalpha;

        transform.localScale = Vector3.one * GameUtil.explosionRadius * 2;

        linerenderer.line.SetColors(color_init, color_init);
    }

    void Update()
    {
        counter += Time.deltaTime;
        Color current = Color.Lerp(color_init, color_noalpha, alpha_curve.Evaluate(counter / explosionTime));
        renderer.color = Color.Lerp(color_halfalpha, color_noalpha, alpha_curve.Evaluate(counter / explosionTime));
        linerenderer.line.SetColors(current, current);
        if (counter >= explosionTime)
        {
            Kill();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (!hasCollided && other.tag == "Explosion")
        {
            Explosion otherExplosion = other.GetComponent<Explosion>();
            if (otherExplosion.id != id)
            {
                Vector3 hv = other.transform.position - transform.position;
                Vector3 pos = transform.position + (hv * 0.5f);

                Vector2 intersect_a, intersect_b;
                int intersect = CircleIntersections(new Vector3(transform.position.x, transform.position.y, GameUtil.explosionRadius),
                                                    new Vector3(otherExplosion.transform.position.x, otherExplosion.transform.position.y, GameUtil.explosionRadius),
                                                    out intersect_a, out intersect_b);


                List<Vector3> points_a = (id == PlayerId.P1) ? this.GetIntersectionPoints((Vector3)intersect_b, (Vector3)intersect_a) :
                                                                  this.GetIntersectionPoints((Vector3)intersect_a, (Vector3)intersect_b);
                List<Vector3> points_b = (id == PlayerId.P1) ? otherExplosion.GetIntersectionPoints((Vector3)intersect_a, (Vector3)intersect_b) :
                                                                 otherExplosion.GetIntersectionPoints((Vector3)intersect_b, (Vector3)intersect_a);
                
                points_a.AddRange(points_b);
               if(points_a.Count > 0) GenerateLine(points_a);
               if(points_b.Count > 0)  otherExplosion.GenerateLine(points_b);

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
                other.GetComponent<Bullet>().Desintegrate();
            }
        }
    }

    public List<Vector3> GetIntersectionPoints(Vector3 point_a, Vector3 point_b)
    {
        List<WavePoint> linepoints = new List<WavePoint>();
        linepoints.AddRange(linerenderer.GetComponent<Soundwave>().linepoints);

        WavePoint a = linepoints.OrderBy(p => Vector3.Distance(p.initialpoint, point_a)).First();
        WavePoint b = linepoints.OrderBy(p => Vector3.Distance(p.initialpoint, point_b)).First();
        Debug.Log(a);
        int startindex = linepoints.IndexOf(a);
        int endindex = linepoints.IndexOf(b);

        if(endindex < startindex) endindex = startindex;
        else if(endindex == startindex) return new List<Vector3>();

        Debug.Log("getting point line from index " + startindex + " to index " + endindex);

        List<Vector3> final = new List<Vector3>();
        for (int i = startindex; i < endindex; i++)
        {
            final.Add(linepoints[i].initialpoint);
        }
        return final;
    }

    public void GenerateLine(List<Vector3> transforms, Color? c = null)
    {
        Color targcol = c ?? Color.white;

        GameObject line = Instantiate(LinePrefab);
        line.GetComponent<LineRenderer>().SetColors(targcol, targcol);
        line.transform.position = this.transform.position;
        line.transform.SetParent(this.transform);

        for (int i = 0; i < transforms.Count; i++)
        {
            GameObject point = Instantiate(LinePointPrefab);
            point.transform.position = transforms[i];
            point.transform.SetParent(line.transform);
        }
    }

    private void Kill()
    {
        if (OnDead != null) OnDead();
        GameObject.Destroy(gameObject);
    }

    // Find the points where the two circles intersect.
    private int CircleIntersections(
        Vector3 circleA, Vector3 circleB,
        out Vector2 intersection1, out Vector2 intersection2)
    {
        // Find the distance between the centers.
        float dx = circleA.x - circleB.x;
        float dy = circleA.y - circleB.y;
        double dist = Math.Sqrt(dx * dx + dy * dy);

        // See how many solutions there are.
        if (dist > circleA.z + circleB.z)
        {
            // No solutions, the circles are too far apart.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if (dist < Math.Abs(circleA.z - circleB.z))
        {
            // No solutions, one circle contains the other.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if ((dist == 0) && (circleA.z == circleB.z))
        {
            // No solutions, the circles coincide.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else
        {
            // Find a and h.
            double a = (circleA.z * circleA.z -
                circleB.z * circleB.z + dist * dist) / (2 * dist);
            double h = Math.Sqrt(circleA.z * circleA.z - a * a);

            // Find P2.
            double cx2 = circleA.x + a * (circleB.x - circleA.x) / dist;
            double cy2 = circleA.y + a * (circleB.y - circleA.y) / dist;

            // Get the points P3.
            intersection1 = new Vector2(
                (float)(cx2 + h * (circleB.y - circleA.y) / dist),
                (float)(cy2 - h * (circleB.x - circleA.x) / dist));
            intersection2 = new Vector2(
                (float)(cx2 - h * (circleB.y - circleA.y) / dist),
                (float)(cy2 + h * (circleB.x - circleA.x) / dist));

            // See if we have 1 or 2 solutions.
            if (dist == circleA.z + circleB.z) return 1;
            return 2;
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