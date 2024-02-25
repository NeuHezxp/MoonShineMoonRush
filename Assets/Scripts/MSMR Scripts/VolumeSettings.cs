using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        SetMusicVolume(); // makes audio compatible
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        masterMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        masterMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SetMusicVolume();
    }
}
