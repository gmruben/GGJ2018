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

    public string JoyL;
    public string JoyR;
    public string JoyFire;
    public string JoyChangeColour;

    public PlayerId id;
    public float angularSpeed = 50;

    public WaveColour[] colours;
    public int currentColourIndex;
    public MeshRenderer renderer;
    public SpriteRenderer line;

    public bool useRelease;
    private Bullet currentBullet;
    public bool keyboardinput;

    void Awake ()
    {
        UpdateColour();
    }


    void Update()
    {
        if(keyboardinput)
        {
            KeyboardInput();
        }
        else JoyPadInput();
        
    }

    void KeyboardInput()
    {
        if (Input.GetKey(up))
        {
            transform.Rotate(Vector3.forward, -angularSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(down))
        {
            transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown (fire))
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

        if (Input.GetKeyDown (colour))
        {
            currentColourIndex++;
            if (currentColourIndex >= colours.Length)
            {
                currentColourIndex = 0;
            }
            UpdateColour();
        }
    }

    void JoyPadInput()
    {
       if (Input.GetButton(JoyR))
        {
            transform.Rotate(Vector3.forward, -angularSpeed * Time.deltaTime);
        }
        else if (Input.GetButton(JoyL))
        {
            transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown(JoyFire))
        {
            currentBullet = GameObject.Instantiate(bulletPrefab).GetComponent<Bullet>();
            currentBullet.transform.position = transform.position;

            currentBullet.Init(id, CurrentColour, transform.up);
        }
        else if (Input.GetButtonUp (JoyFire))
        {
            if (currentBullet != null && !currentBullet.hasDesintegrated)
            {
                currentBullet.Explode();
                currentBullet = null;
            }
        }

        if (Input.GetButtonDown (JoyChangeColour))
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
        line.color = GameUtil.GetColor(CurrentColour);
    }
}