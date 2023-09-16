using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SoundEffectsSlider;
    private void Start()
    {
        float MusVol = PlayerPrefs.GetFloat("MusicVolume", -1);
        if (MusVol != -1)
        {
            MusicSlider.value = MusVol;
        }
        float SoEVol = PlayerPrefs.GetFloat("SoundEffectVolume", -1);
        if (SoEVol != -1)
        {
            SoundEffectsSlider.value = SoEVol;
        }
    }
    public void MusicControl()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        PlayMusic[] pM = FindObjectsOfType<PlayMusic>();
        foreach(PlayMusic M in pM)
        {
            M.FixedUpdate();
        }
    }
    public void SoundEffectControl()
    {
        PlayerPrefs.SetFloat("SoundEffectVolume", SoundEffectsSlider.value);
        PlayMusic[] pM = FindObjectsOfType<PlayMusic>();
        foreach (PlayMusic M in pM)
        {
            M.FixedUpdate();
        }
    }
}
