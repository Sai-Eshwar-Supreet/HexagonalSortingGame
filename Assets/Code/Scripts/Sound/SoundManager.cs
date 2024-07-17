using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource themeAudioSource;
    [SerializeField] private AudioSource uiSfxAudioSource;
    [SerializeField] private AudioSource environmentSfxAudioSource;

    private static SoundManager instance;
    public static readonly string MasterVolumeName = "Master Volume";
    public static readonly string MusicVolumeName = "Music Volume";
    public static readonly string SfxVolumeName = "Sfx Volume";
    public static readonly string UISfxVolumeName = "UI Sfx Volume";
    public static readonly string EnvironmentSfxVolumeName = "Environment Sfx Volume";
    public static readonly string AmbienceVolumeName = "Ambience Volume";

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    public void SetVolume(string soundGroup, float volume)
    {
        volume = Mathf.Clamp(volume, -80f, 0);
        audioMixer.SetFloat(soundGroup, volume);
    }

    public static void PlayUISfx(AudioClip clip, float volume)
    {
        instance.uiSfxAudioSource.PlayOneShot(clip, volume);
    }
    public static void StopUISfx()
    {
        instance.uiSfxAudioSource.Stop();
    }
    public static void PlayEnvironmentSfx(AudioClip clip, float volume)
    {
        instance.environmentSfxAudioSource.PlayOneShot(clip, volume);
    }
    public static void StopEnvironmentSfx()
    {
        instance.environmentSfxAudioSource.Stop();
    }
}
