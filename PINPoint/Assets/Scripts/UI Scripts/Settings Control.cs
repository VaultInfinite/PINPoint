using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the settings methods
public class SettingsControl : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider effectsSlider;
    [SerializeField]
    private Slider ambientSlider;
    private float masterVolume;

    

    public void MasterVolumeControl()
    {
        masterVolume = masterSlider.value;
        AdjustVolume();
    }

    public void MusicVolumeControl()
    {
        AudioControl.Instance.musicAudio.volume = musicSlider.value * masterSlider.value;
    }

    public void EffectsVolumeControl()
    {
        AudioControl.Instance.effectsAudio.volume = effectsSlider.value * masterSlider.value;
    }

    public void AmbientVolumeControl()
    {
        AudioControl.Instance.ambientAudio.volume = ambientSlider.value * masterSlider.value;
    }

    /// <summary>
    /// Applies the master volume to all audio sources
    /// </summary>
    private void AdjustVolume()
    {
        AudioControl.Instance.musicAudio.volume = musicSlider.value * masterSlider.value;
        AudioControl.Instance.effectsAudio.volume = effectsSlider.value * masterSlider.value;
        AudioControl.Instance.ambientAudio.volume = ambientSlider.value * masterSlider.value;
    }
}
