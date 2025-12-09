using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum sfxType
{
    JUMP,
    SHOOT,
    FOOTSTEP
}

public enum musicType
{
    MainMenu,
    Level
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
    public AudioClip[] musicList;
    public AudioClip mainMenuClip;
    public AudioClip levelClip;

    [Header("Ambient")]
    public AudioClip ambientClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //Keep this object even when changing scenes
        DontDestroyOnLoad(gameObject);
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

    public void PlayMusic(musicType music)
    {
        Debug.Log("Play Music");
        instance.musicAudio.clip = instance.musicList[(int)music];

        instance.musicAudio.Play();
    }

    public void StopMusic()
    {
        Debug.Log("Stop Music");

        instance.effectsAudio.Stop();
    }

    private void PlayAmbient()
    {
    }

    public void PlaySceneMusic()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        Debug.Log("Scene: " + sceneName);

        switch (sceneName)
        {
            case "StartMenu":

                PlayMusic(musicType.MainMenu);


                break;

            case "Hideout":
                PlayMusic(musicType.MainMenu);

                break;

            case "FinalLevel":
                PlayMusic(musicType.Level);

                break;

            default:

                Debug.LogErrorFormat("ERROR: SCENE NAME");

                break;
        }
    }
}
