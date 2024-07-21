using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [Space(20)]
    [Header("UI")]
    [SerializeField] private Toggle muteMusicToggle;
    [SerializeField] private Toggle muteAllToggle;
    [SerializeField] private Toggle muteSfxToggle;

    [Header("Min Max Value")]
    [SerializeField] private AnimationCurve minMaxInterpolationCurve;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    private void OnEnable()
    {
        muteAllToggle.onValueChanged.AddListener(MasterVolumeChangeHandler);
        muteMusicToggle.onValueChanged.AddListener(MusicVolumeChangeHandler);
        muteSfxToggle.onValueChanged.AddListener(SfxVolumeChangeHandler);
    }
    private void OnDisable()
    {
        muteAllToggle.onValueChanged.RemoveListener(MasterVolumeChangeHandler);
        muteMusicToggle.onValueChanged.RemoveListener(MusicVolumeChangeHandler);
        muteSfxToggle.onValueChanged.RemoveListener(SfxVolumeChangeHandler);
    }
    private void MasterVolumeChangeHandler(bool status)
    {
        float value = status ? minValue : maxValue;
        soundManager.SetVolume(SoundManager.MasterVolumeName, value);
    }
    private void MusicVolumeChangeHandler(bool status)
    {
        float value = status ? minValue : maxValue;
        soundManager.SetVolume(SoundManager.MusicVolumeName, value);
    }
    private void SfxVolumeChangeHandler(bool status)
    {
        float value = status ? minValue : maxValue;
        soundManager.SetVolume(SoundManager.SfxVolumeName, value);
    }
}
