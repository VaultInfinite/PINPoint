using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Contracts;



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

    [Header("Window")]
    public TMP_Dropdown windowMode;
    public TMP_Dropdown resolutionMode;

    private bool fullScreen;

    [Header("Aim")]
    [SerializeField]
    private Slider aimSensitivitySlider;
    [SerializeField]
    private Slider camSensitivitySlider;
    public Toggle reverseAim;


    private void Start()
    {
        fullScreen = true;
        Screen.SetResolution(1280, 1240, fullScreen);
    }

    #region Volume Methods

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

    #endregion

    #region Window Methods

    public void ChangeWindowType()
    {
        switch (windowMode.value)
        {
            case 0: // Full Screen
                fullScreen = true;
                ChangeResolution();
                break;


            case 1: // Window
                fullScreen = false;
                ChangeResolution();
                break;
        }
    }

    public void ChangeResolution()
    {
        switch (resolutionMode.value)
        {
            case 0: // SD
                
                Screen.SetResolution(720, 480, fullScreen);

                break;

            case 1: // HD

                Screen.SetResolution(1280, 768, fullScreen);

                break;


            case 2: // Full HD

                Screen.SetResolution(1280, 1240, fullScreen);

                break;

            case 3: // 2k Quad HD

                Screen.SetResolution(1920, 1080, fullScreen);

                break;

            case 4:

                Screen.SetResolution(2560, 1440, fullScreen);

                break;

        }
    }

    #endregion

    #region Controls Methods

    /// <summary>
    /// Controls the Aim Sensitivity
    /// </summary>
    public void ControlAimSensitivity()
    {
        CameraControl.Instance.aimX = aimSensitivitySlider.value;
        CameraControl.Instance.aimY = aimSensitivitySlider.value;
    }

    /// <summary>
    /// Control the general camera sensitivity
    /// </summary>
    public void ControlCamSensitivity()
    {
        CameraControl.Instance.sensX = camSensitivitySlider.value;
        CameraControl.Instance.sensY = camSensitivitySlider.value;
    }

    public void ReverseAim()
    {
        CameraControl.Instance.inverse *= -1;
    }

    #endregion
}
