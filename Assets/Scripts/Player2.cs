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

    public string JoyFire;

    public string aimX;
    public string aimY;
    public string colorX;
    public string colorY;

    public PlayerId id;
    public float angularSpeed = 50;
    
    public MeshRenderer renderer;
    public SpriteRenderer line;
    
    private Bullet currentBullet;

    public Transform aimArrow;
    public Transform colorArrow;

    void Start ()
    {
        UpdateColour();
    }

    void Update()
    {
        if (Input.GetButtonDown(JoyFire))
        {
            if (id == PlayerId.P1) AudioManager.PlaySFX("P1Shoots");
            else AudioManager.PlaySFX("P2Shoots");

            currentBullet = GameObject.Instantiate(bulletPrefab).GetComponent<Bullet>();
            currentBullet.transform.position = transform.position;

            currentBullet.Init(id, CurrentColour, aimArrow.up);
        }
        else if (Input.GetButtonUp(JoyFire))
        {
            if (currentBullet != null && !currentBullet.hasDesintegrated)
            {
                currentBullet.Explode();
                currentBullet = null;
            }
        }

        float ax = Input.GetAxis(aimX);
        float ay = -Input.GetAxis(aimY);
        float cx = Input.GetAxis(colorX);
        float cy = -Input.GetAxis(colorY);

        Vector3 aimVector = new Vector3(ax, ay, 0.0f);
        Vector3 colorVector = new Vector3(cx, cy, 0.0f);

        if (aimVector.sqrMagnitude > 0.5f)
        {
            aimArrow.rotation = Quaternion.FromToRotation(Vector3.up, aimVector);
        }
        if (colorVector.sqrMagnitude > 0.5f)
        {
            colorArrow.rotation = Quaternion.FromToRotation(Vector3.up, colorVector);
        }
    }

    public WaveColour CurrentColour
    {
        get
        {
            float angle = colorArrow.transform.eulerAngles.z;

            if (angle >= 60 && angle < 180) return WaveColour.Red;
            else if (angle >= 180 && angle < 300) return WaveColour.Yellow;
            else return WaveColour.Blue;
        }
    }

    private void UpdateColour ()
    {
        renderer.material.SetColor("_Color", GameUtil.GetColor(CurrentColour));
        line.color = GameUtil.GetColor(CurrentColour);
    }
}