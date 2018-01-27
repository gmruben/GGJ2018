using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    private float shakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder.
    private float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public static CameraShake instance;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
        instance = this;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void Shake (float intensity, float duration)
    {
        shakeAmount = intensity;
        shakeDuration = duration;
    }
}