using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveParticle : MonoBehaviour
{
    public GameObject explosionPrefab;

    public PlayerId owner;
    public Vector3 direction;
    public bool hasCollided;
    public GameObject parent;

    public bool useExplosion;
    private float speed;

    public void Init (PlayerId owner, GameObject parent, Vector3 direction, float speed)
	{
        this.owner = owner;
        this.parent = parent;
		this.direction = direction;
        this.speed = speed;
	}

    void Update ()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        WaveParticle otherParticle = other.GetComponent<WaveParticle>();

        if (!hasCollided && otherParticle != null && otherParticle.owner != owner)
        {
            otherParticle.hasCollided = true;
            hasCollided = true;
            
            GameObject.Destroy(parent);
            GameObject.Destroy(otherParticle.parent);

            GameObject explosion = GameObject.Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
        }
    }
}