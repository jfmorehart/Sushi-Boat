using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeFrequency = 10.0f; 

    private Vector3 originalPos;
    private bool shaking = false;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shaking)
        {
            float xOffset = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2 - 1;
            float yOffset = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2 - 1;

            transform.localPosition = originalPos + new Vector3(xOffset/4f, yOffset, 0) * shakeMagnitude;
        }
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake()); // Or whatever duration you prefer
    }

    public IEnumerator Shake()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
        transform.localPosition = originalPos;
    }
}
