using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 书店的音乐台
/// </summary>
public class Station : MonoBehaviour
{
    [SerializeField] private AudioControl audioControl;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject pause;
    private int number = 0;

    public int Number
    {
        get => number;
        set
        {
            while (value<0)
            {
                value += audioClips.Length;
            }
            value %= audioClips.Length;
            number = value;
        }
    }

    private void OnEnable()
    {
        audioControl.StopPlayAll();
        audioControl.PlayLoop(audioClips[0],AudioControl.BackgroundMusicType.BookStore);
        play.SetActive(false);
        pause.SetActive(true);
    }

    public void Stop()
    {
        audioControl.StopPlayAll();
    }

    public void Next()
    {
        Number++;
        audioControl.PlayLoop(audioClips[Number],AudioControl.BackgroundMusicType.BookStore);
    }

    public void Last()
    {
        Number--;
        audioControl.PlayLoop(audioClips[Number],AudioControl.BackgroundMusicType.BookStore);
    }

    public void OnStart()
    {
        if (audioControl.audioSource.isPlaying)
        {
            audioControl.audioSource.Pause();
            play.SetActive(true);
            pause.SetActive(false);
        }
        else
        {
            audioControl.audioSource.Play();
            play.SetActive(false);
            pause.SetActive(true);
        }
    }
}