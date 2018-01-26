using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode fire;
    public KeyCode up;
    public KeyCode down;

    public TowerId id;

    private bool inCone;
    private float coneCounter;

    public Transform leftMask;
    public Transform rightMask;
    public GameObject cone;

    public GameObject waveParticlePrefab;

    public float minSpeed = 1;
    public float maxSpeed = 10;

    public float angularSpeed = 5;

    void Awake ()
    {
        cone.SetActive(false);
    }

    void Update ()
    {
        if (Input.GetKey (down))
        {
            transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(up))
        {
            transform.Rotate(Vector3.forward, -angularSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown (fire))
        {
            inCone = true;
            coneCounter = 0.0f;
            cone.SetActive (true);
        }
        else if (Input.GetKey (fire))
        {
            coneCounter = Mathf.Clamp(coneCounter + Time.deltaTime, 0.0f, 0.95f);
        }
        else if (Input.GetKeyUp(fire))
        {
            inCone = false;
            cone.SetActive(false);

            float speed = Mathf.Lerp(minSpeed, maxSpeed, coneCounter);
            Fire((int) (90.0f * coneCounter), speed);
        }

        float angle = 90.0f * coneCounter;

        leftMask.localRotation = Quaternion.Euler(0, 0, -angle);
        rightMask.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void Fire (int angle, float speed)
    {
        GameObject wave = new GameObject("Wave");
        int na = 90 - angle;
        for (int i = -na; i < na; i++)
        {
            Vector3 vector = Quaternion.AngleAxis(i, Vector3.forward) * transform.up;

            WaveParticle waveParticle = GameObject.Instantiate(waveParticlePrefab).GetComponent<WaveParticle>();
            waveParticle.Init(id, wave, vector, speed);

            waveParticle.transform.position = transform.position;
            waveParticle.transform.SetParent(wave.transform);
        }
    }
}