using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public MeshRenderer renderer;
    public WaveColour colour;

    public void Init(WaveColour colour)
    {
        this.colour = colour;

        renderer.material.SetColor("_Color", GameUtil.GetColor(colour));
    }

    void Update ()
    {
        transform.position -= Vector3.up * speed * Time.deltaTime;
	}

    public void Kill ()
    {
        GameObject.Destroy(gameObject);
    }
}