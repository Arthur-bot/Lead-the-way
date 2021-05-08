using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceAudio : MonoBehaviour
{
    #region Singleton
    public static UserInterfaceAudio Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    [SerializeField] AudioClip onMenuOpen, onMenuClose;
    [SerializeField] AudioSource audioSource;

    void Play(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void PlayClip(AudioClip audioClip)
    {
        if (Instance != null) Instance.Play(audioClip);
    }
}
