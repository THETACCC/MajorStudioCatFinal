using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxsounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        PlayMusic("BGM");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.Audioname == name);

        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxsounds, x => x.Audioname == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            // Set a random pitch between a defined range
            float randomPitch = UnityEngine.Random.Range(0.9f, 1.1f); // Adjust the range as needed
            sfxSource.pitch = randomPitch;

            // Play the sound clip
            sfxSource.PlayOneShot(s.clip);

            // Reset the pitch to default (1.0f) to avoid affecting subsequent sounds
            //sfxSource.pitch = 1.0f;
        }

    }

}
