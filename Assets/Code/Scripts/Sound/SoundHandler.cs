using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField, Range(0, 1)] private float clickVolume = 1;
    public void PlaySound()
    {
        SoundManager.PlayUISfx(clickSound, clickVolume);
    }
}
