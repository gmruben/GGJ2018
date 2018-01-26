using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

	void Update ()
    {
        transform.position -= Vector3.up * speed * Time.deltaTime;
	}

    public void Kill ()
    {
        GameObject.Destroy(gameObject);
    }
}