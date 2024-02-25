using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance;

    public Sound[] musicSounds, SFXSounds;
    public AudioSource musicSource, SFXSource;

    private void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string songName)
    {
        Sound s = Array.Find(musicSounds, x => x.name == songName);

        if (s == null)
        {
            Debug.Log("Song name not found");
        }
        else
        { 
            musicSource.clip = s.clip;
            musicSource.Play(); 
        }
    }

    public void PlaySFX(string sfxName)
    { 
        Sound s = Array.Find(SFXSounds, x => x.name == sfxName);

        if (s == null)
        {
            Debug.Log("SFX name not found");
        }
        else 
        {
            SFXSource.PlayOneShot(s.clip);
        }
    }
}
