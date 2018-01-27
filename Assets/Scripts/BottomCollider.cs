using System;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    public event Action OnBottomReached;
    public GameObject particleSystemPrefab;

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameObject explosion = GameObject.Instantiate(particleSystemPrefab);
            explosion.transform.position = other.transform.position;

            GameObject.Destroy (other.gameObject);
            CameraShake.instance.Shake(0.75f, 1.0f);

            if (OnBottomReached != null) OnBottomReached();
        }
    }
}