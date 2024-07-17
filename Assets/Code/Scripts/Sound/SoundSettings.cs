using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [Space(20)]
    [Header("UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider environmentSoundsSlider;
    [SerializeField] private Slider uiSoundsSlider;
    [SerializeField] private Slider ambienceSoundsSlider;
    [SerializeField] private Toggle muteAllToggle;
    [SerializeField] private Toggle muteSfxToggle;

    [Header("Min Max Value")]
    [SerializeField] private AnimationCurve minMaxInterpolationCurve;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    private void OnEnable()
    {
        muteAllToggle.onValueChanged.AddListener(MasterVolumeChangeHandler);
        musicSlider.onValueChanged.AddListener(MusicVolumeChangeHandler);
        muteSfxToggle.onValueChanged.AddListener(SfxVolumeChangeHandler);
        environmentSoundsSlider.onValueChanged.AddListener(EnvironmentVolumeChangeHandler);
        uiSoundsSlider.onValueChanged.AddListener(UIVolumeChangeHandler);
        ambienceSoundsSlider.onValueChanged.AddListener(AmbienceVolumeChangeHandler);
    }
    private void OnDisable()
    {
        muteAllToggle.onValueChanged.RemoveListener(MasterVolumeChangeHandler);
        musicSlider.onValueChanged.RemoveListener(MusicVolumeChangeHandler);
        muteSfxToggle.onValueChanged.RemoveListener(SfxVolumeChangeHandler);
        environmentSoundsSlider.onValueChanged.RemoveListener(EnvironmentVolumeChangeHandler);
        uiSoundsSlider.onValueChanged.RemoveListener(UIVolumeChangeHandler);
        ambienceSoundsSlider.onValueChanged.RemoveListener(AmbienceVolumeChangeHandler);
    }
    private void MasterVolumeChangeHandler(bool status)
    {
        float value = status ? minValue : maxValue;
        soundManager.SetVolume(SoundManager.MasterVolumeName, value);
    }
    private void MusicVolumeChangeHandler(float value)
    {
        soundManager.SetVolume(SoundManager.MusicVolumeName, GetVolume(value));
    }
    private void SfxVolumeChangeHandler(bool status)
    {
        float value = status ? minValue : maxValue;
        soundManager.SetVolume(SoundManager.SfxVolumeName, value);
    }
    private void EnvironmentVolumeChangeHandler(float value)
    {
        soundManager.SetVolume(SoundManager.EnvironmentSfxVolumeName, GetVolume(value));
    }
    private void UIVolumeChangeHandler(float value)
    {
        soundManager.SetVolume(SoundManager.UISfxVolumeName, GetVolume(value));
    }
    private void AmbienceVolumeChangeHandler(float value)
    {
        soundManager.SetVolume(SoundManager.AmbienceVolumeName, GetVolume(value));
    }
    private float GetVolume(float value) => Mathf.Lerp(minValue, maxValue, minMaxInterpolationCurve.Evaluate(value));
}
