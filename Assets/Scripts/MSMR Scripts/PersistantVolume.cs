using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistantVolume : MonoBehaviour
{
    public Slider m_volume;
    
    private void Awake()
    {
        if (m_volume != null && PlayerPrefs.HasKey("MusicVolume"))
        {
            float wantedVolume = PlayerPrefs.GetFloat("MusicVolume");

            m_volume.value = wantedVolume;

            m_volume.onValueChanged.AddListener(delegate { SetGameVolume(m_volume.value); });
        }
    }

    public void SetGameVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
