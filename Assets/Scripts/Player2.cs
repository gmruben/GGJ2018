using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public GameObject bulletPrefab;

    public KeyCode fire;
    public KeyCode up;
    public KeyCode down;

    public ExplosionColour colour;
    public TowerId id;
    public float angularSpeed = 50;

    private Bullet currentBullet;

    void Update()
    {
        if (Input.GetKey(down))
        {
            transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(up))
        {
            transform.Rotate(Vector3.forward, -angularSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(fire))
        {
            if (currentBullet != null)
            {
                currentBullet.Explode();
                currentBullet = null;
            }
            else
            {
                currentBullet = GameObject.Instantiate(bulletPrefab).GetComponent<Bullet> ();
                currentBullet.transform.position = transform.position;

                currentBullet.Init (id, colour, transform.up);
            }
        }
    }
}