using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public float freq, amp, noisefreq, noiseamp;

    public bool ringing = false;

    public AudioClip bellSound;

    public Transform bellIcon;
    CameraController cam;
    // Update is called once per frame
    void Update()
    {
        cam = Camera.main.GetComponent<CameraController>();
        float noise = Mathf.PerlinNoise1D(Time.time * noisefreq) * noiseamp;

        float sin = Mathf.Sin(Time.time * freq) * amp;
        float deg = (sin - 0.5f) + (noise - 0.5f);
        if (ringing)
        {
            transform.GetChild(1).eulerAngles = new Vector3(0, 0, deg);
            if(cam.trackingHook&& bellIcon.gameObject.activeSelf)
                bellIcon.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, deg);
        }
        else
        {
            transform.GetChild(1).eulerAngles = new Vector3(0, 0, 0);
            if(cam.trackingHook&& bellIcon.gameObject.activeSelf)
                bellIcon.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }

        
    }

    public void Ring()
    {
        StartCoroutine(RingCouroutine());
    }

    IEnumerator RingCouroutine()
    {
        if (cam.trackingHook)
        {
            bellIcon.gameObject.SetActive(true);
            bellIcon.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }
        ringing = true;
        SoundManager.Instance.PlaySoundEffect(bellSound);
        yield return new WaitForSeconds(2f);
        ringing = false;
        if (cam.trackingHook)
            bellIcon.gameObject.SetActive(false);
    }
    
    
}
