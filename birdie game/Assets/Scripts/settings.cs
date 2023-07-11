using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class settings : MonoBehaviour
{
    public AudioMixer mainMixer;

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
