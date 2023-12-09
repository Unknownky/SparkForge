using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class audiomanager : MonoBehaviour
{
    private static audiomanager instance;

    public AudioClip[] audioClips;

    public static audiomanager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<audiomanager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "audiomanager";
                    instance = obj.AddComponent<audiomanager>();
                }
            }
            return instance;
        }
    }
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
        // Get the current scene name
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Play corresponding music based on the scene name
        switch (sceneName)
        {
            case "Level_0-1":
                PlaySound(audioClips[0]);
                break;
            case "Level_1-1":
                PlaySound(audioClips[1]);
                break;
            case "Level_4-1":
                PlaySound(audioClips[2]);
                break;
            default:
                // Play default music or handle the case when the scene name doesn't match any specific music
                break;
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 该方法播放推箱子的声音
    /// </summary>
    public void PlayPushBoxSound()
    {
        audioSource.PlayOneShot(audioClips[3]);
    }   

    public void StopSound()
    {
        audioSource.Stop();
    }

}