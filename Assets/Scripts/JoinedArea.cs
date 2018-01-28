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
    private int numKilledEnemies;

    float numSteps = 100;

    public void Init (Explosion e1, Explosion e2)
    {
        this.e1 = e1;
        this.e2 = e2;

        pos1 = e1.transform.position;
        pos2 = e2.transform.position;

        e1.OnDead += HandleOnExplosionDead;
        e2.OnDead += HandleOnExplosionDead;

        colour = GameUtil.GetCombinedColor(e1.colour, e2.colour);
        numKilledEnemies = 0;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            float d1 = (other.transform.position - pos1).sqrMagnitude;
            float d2 = (other.transform.position - pos2).sqrMagnitude;

            Enemy enemy = other.GetComponent<Enemy>();
            if (CheckIsPointInArea(enemy.transform.position))
            {
                CheckKillEnemy(enemy);
            }
            else
            {
                float step = (Mathf.PI * 2) / numSteps;
                for (int i = 0; i < numSteps; i++)
                {
                    float angle = i * step * Mathf.Rad2Deg;

                    Vector3 vector = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
                    Vector3 point = enemy.transform.position + vector.normalized * enemy.radius;

                    if (CheckIsPointInArea(enemy.transform.position))
                    {
                        CheckKillEnemy(enemy);
                        break;
                    }
                }
            }
        }
    }

    private bool CheckIsPointInArea (Vector3 point)
    {
        float d1 = (point - pos1).sqrMagnitude;
        float d2 = (point - pos2).sqrMagnitude;

        return (d1 <= GameUtil.ExplosionRadiusSquared && d2 <= GameUtil.ExplosionRadiusSquared);
    }

    private void HandleOnExplosionDead ()
    {
        e1.OnDead -= HandleOnExplosionDead;
        e2.OnDead -= HandleOnExplosionDead;

        GameObject.Destroy(gameObject);
    }

    private void CheckKillEnemy (Enemy enemy)
    {
        if (enemy.colour == colour)
        {
            numKilledEnemies++;
            enemy.Kill(numKilledEnemies);
        }
    }
}