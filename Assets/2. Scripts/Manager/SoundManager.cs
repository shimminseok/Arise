using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BGM
{
}

public enum SFX
{
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private List<AudioClip> BGMClips;
    [SerializeField] private List<AudioClip> SFXClips;


    public void ChangeBGM(BGM bgm)
    {
        AudioClip clip = BGMClips[(int)bgm];
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    public void PlaySFX(SFX sfx)
    {
        SFXSource.PlayOneShot(SFXClips[(int)sfx]);
    }

    public void ChangeBGMVolume(float volume)
    {
        BGMSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}