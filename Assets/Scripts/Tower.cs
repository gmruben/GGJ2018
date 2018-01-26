using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public KeyCode keyCode;
    public TowerId id;

    public GameObject waveParticlePrefab;

	void Update ()
	{
		if (Input.GetKeyDown (keyCode))
		{
            EmitWave();
        }
	}

	private void EmitWave ()
	{
        float numSteps = 100;
        float step = (Mathf.PI * 2) / numSteps;

		for (int i = 0; i < numSteps; i++)
		{
            float angle = i * step * Mathf.Rad2Deg;
			Vector3 vector = Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.up;

            WaveParticle waveParticle = GameObject.Instantiate (waveParticlePrefab).GetComponent<WaveParticle> ();
			//waveParticle.Init (id, vector);

            waveParticle.transform.position = transform.position;
        }
	}
}

public enum TowerId { Left, Right }