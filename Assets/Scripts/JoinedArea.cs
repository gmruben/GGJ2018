using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinedArea : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;

    private Explosion e1;
    private Explosion e2;

    public void Init (Explosion e1, Explosion e2)
    {
        this.e1 = e1;
        this.e2 = e2;

        pos1 = e1.transform.position;
        pos2 = e2.transform.position;

        e1.OnDead += HandleOnExplosionDead;
        e2.OnDead += HandleOnExplosionDead;
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Enemy")
        {
            float d1 = (other.transform.position - pos1).sqrMagnitude;
            float d2 = (other.transform.position - pos2).sqrMagnitude;

            Debug.Log("DISTANCE: " + d1 + " - " + d2);
            if (d1 <= GameUtil.explosionRadius && d2 <= GameUtil.explosionRadius)
            {
                other.GetComponent<Enemy>().Kill();
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