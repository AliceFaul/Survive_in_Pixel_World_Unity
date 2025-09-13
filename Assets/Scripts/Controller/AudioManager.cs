using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;
    public AudioClip[] musicClips, sfxClips;

    private int index;

    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = UnityEngine.Random.Range(0, musicClips.Length);
        PlayMusic(index);
    }

    public void PlayMusic(int index)
    {
        if (index < musicClips.Length)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = Array.Find(sfxClips, x => x.name == name);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
