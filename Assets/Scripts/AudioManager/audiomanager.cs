using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class audiomanager : MonoBehaviour
{
    public static audiomanager Instance;

    public Audiosmassages[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public AudioClip audioClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance =  this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        #if UNITY_EDITOR
        Debug.Log("Unity Editor");
        #endif
        PlayMusic("csyx");//背景音乐，csyx就是测试音效，换了就行
    }


    public void PlayMusic(string name)
    {
        Audiosmassages s = Array.Find(musicSounds,x => x.name == name);

        if (s == null)
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
        Audiosmassages s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
