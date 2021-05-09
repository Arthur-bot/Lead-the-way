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

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioMusic;

    AudioSource audioSource2;

    bool MuteMusic;
    bool MuteSfx;
    string muteMusicKey = "MuteMusic";
    string muteSfxKey = "MuteSFX";

    private void Start()
    {
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.spatialBlend = 0;
        audioSource2.clip = audioMusic;
        audioSource2.loop = true;
        audioSource2.volume = 0.2f;
        audioSource2.Play();


        LoadSettings();

        audioSource.mute = MuteSfx;
        audioSource2.mute = MuteMusic;
    }

    void Play(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void SFX()
    {
        audioSource.mute = !audioSource.mute;
        MuteSfx = !audioSource.mute;

        PlayerPrefs.SetInt(muteSfxKey, MuteSfx ? 1 : 0);
    }

    public void Music()
    {
        audioSource2.mute = !audioSource2.mute;
        MuteMusic = !audioSource2.mute;

        PlayerPrefs.SetInt(muteMusicKey, MuteMusic ? 1 : 0);
    }

    public static void PlayClip(AudioClip audioClip)
    {
        if (Instance != null) Instance.Play(audioClip);
    }

    void LoadSettings()
    {
        MuteMusic = PlayerPrefs.GetInt(muteMusicKey, 0) == 1;
        MuteSfx = PlayerPrefs.GetInt(muteSfxKey, 0) == 1;
    }
}
