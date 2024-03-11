using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public AudioSource BackGroundMusicSource;
    public GameObject soundEffectObject;

    public AudioClip defaultPickupSound;
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        GameObject obj =  Instantiate(soundEffectObject);
        obj.GetComponent<AudioSource>().PlayOneShot(soundEffect);
    }

    public void StartBGM(AudioClip BGM)
    {
        BackGroundMusicSource.clip = BGM;
        BackGroundMusicSource.Play();
    }
}
