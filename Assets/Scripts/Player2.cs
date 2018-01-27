using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public GameObject bulletPrefab;

    public KeyCode fire;
    public KeyCode colour;
    public KeyCode up;
    public KeyCode down;

    public string JoyVertical, JoyFireButton, JoyChangeColor;

    public TowerId id;
    public float angularSpeed = 50;

    public WaveColour[] colours;
    public int currentColourIndex;
    public MeshRenderer renderer;

    private Bullet currentBullet;

    void Awake ()
    {
        UpdateColour();
    }

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

                currentBullet.Init (id, CurrentColour, transform.up);
            }
        }

        if (Input.GetKeyDown(colour))
        {
            currentColourIndex++;
            if (currentColourIndex >= colours.Length)
            {
                currentColourIndex = 0;
            }
            UpdateColour();
        }
    }

    public WaveColour CurrentColour
    {
        get
        {
            return colours [currentColourIndex];
        }
    }

    private void UpdateColour ()
    {
        renderer.material.SetColor("_Color", GameUtil.GetColor(CurrentColour));
    }
}