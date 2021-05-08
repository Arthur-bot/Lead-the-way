using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
