using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum sfxType
{
    JUMP,
    SHOOT,
    FOOTSTEP
}

public enum musicType
{
    MAINMENU,
    STAGE
}

/// <summary>
/// This script is in charge of managing volume
/// </summary>
public class AudioControl : MonoBehaviour
{
    private static AudioControl instance;
    public static AudioControl Instance { get { return instance; } }

    //Variables
    [SerializeField, Header("AudioSource")]
    public AudioSource musicAudio;
    [SerializeField]
    public AudioSource effectsAudio;
    [SerializeField]
    public AudioSource ambientAudio;

    [Header("Sound Effects")]
    public AudioClip[] sfxList;
    public AudioClip jumpClip;
    public AudioClip shootClip;

    [Header("Music")]
    public AudioClip musicClip;

    [Header("Ambient")]
    public AudioClip ambientClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Play sound effect
    /// </summary>
    public void PlaySoundEffect(sfxType sound)
    {
        Debug.Log("Play Sound");
        instance.effectsAudio.clip = instance.sfxList[(int)sound];

        instance.effectsAudio.Play();
    }

    private void PlayMusic()
    {
    }

    private void PlayAmbient()
    {
    }

    
}
