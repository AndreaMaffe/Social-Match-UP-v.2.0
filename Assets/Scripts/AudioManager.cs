﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private AudioClip backgroundMusic;
    private AudioClip ding;
    private AudioClip fireworks;
    private AudioClip hurray;
    private AudioClip victory;
    private AudioClip pop;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        backgroundMusic = Resources.Load<AudioClip>("Sounds/Soundtrack");
        ding = Resources.Load<AudioClip>("Sounds/Ding");
        fireworks = Resources.Load<AudioClip>("Sounds/FireworksSound");
        hurray = Resources.Load<AudioClip>("Sounds/Hurray!");
        victory = Resources.Load<AudioClip>("Sounds/Victory");
        pop = Resources.Load<AudioClip>("Sounds/Pop");

        audioSource.clip = backgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.loop = true;
    }

    public void PlayBackgroundMusic()
    {
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    public void PlayDingSound()
    {
        audioSource.PlayOneShot(ding, 1f);
    }

    public void PlayVictorySound()
    {
        audioSource.PlayOneShot(victory, 4f);
    }

    public void PlayHurraySound()
    {
        audioSource.PlayOneShot(hurray, 6f);
    }

    public void PlayFireworksSound()
    {
        audioSource.PlayOneShot(fireworks, 10f);
    }

    public void PlayPopSound()
    {
        audioSource.PlayOneShot(pop, 1f);
    }

}

