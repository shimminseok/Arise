using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BGM
{
    InGame,
}

public enum SFX
{
    FireArrow_1,
    FireArrow_2,
    Canon_1
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private List<AudioClip> BGMClips;
    [SerializeField] private List<AudioClip> SFXClips;


    protected override void Awake()
    {
        base.Awake();
    }
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