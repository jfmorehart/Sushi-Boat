using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeFrequency = 10.0f; 

    private Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            float xOffset = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2 - 1;
            float yOffset = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2 - 1;

            transform.localPosition = originalPos + new Vector3(xOffset, yOffset, 0) * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 1.5f; // Or whatever duration you prefer
    }
}
