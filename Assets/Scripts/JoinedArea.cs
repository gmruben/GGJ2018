using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinedArea : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;

    private Explosion e1;
    private Explosion e2;

    public WaveColour colour;

    public void Init (Explosion e1, Explosion e2)
    {
        this.e1 = e1;
        this.e2 = e2;

        pos1 = e1.transform.position;
        pos2 = e2.transform.position;

        e1.OnDead += HandleOnExplosionDead;
        e2.OnDead += HandleOnExplosionDead;

        colour = GameUtil.GetCombinedColor(e1.colour, e2.colour);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            float d1 = (other.transform.position - pos1).sqrMagnitude;
            float d2 = (other.transform.position - pos2).sqrMagnitude;

            Enemy enemy = other.GetComponent<Enemy>();
            float radiusSquared = GameUtil.ExplosionRadiusSquared - (enemy.radius * enemy.radius);

            if (d1 <= radiusSquared && d2 <= radiusSquared)
            {
                if (enemy.colour == colour)
                {
                    enemy.Kill();
                }
            }
        }
    }

    private void HandleOnExplosionDead ()
    {
        e1.OnDead -= HandleOnExplosionDead;
        e2.OnDead -= HandleOnExplosionDead;

        GameObject.Destroy(gameObject);
    }
}