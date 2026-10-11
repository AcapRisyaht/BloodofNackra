using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
   public AudioMixerGroup musicMixerGroup;
   public Slider MuzikSlider;
   public Slider SFXSlider;
   public Slider AmbientSlider;

   void Start()
    {
        MuzikSlider.onValueChanged .AddListener ( SetMuzikVolume );
        SFXSlider.onValueChanged.AddListener( SetSFXVolume ) ;
        AmbientSlider.onValueChanged.AddListener( SetAmbientVolume );
    }
    
    public void SetMuzikVolume(float value)
    {
      float dB = (value > 0) ? Mathf.Log10(value) * 20 : -80;
      musicMixerGroup.audioMixer.SetFloat("KelantanganMuzik", dB);
    }

    public void SetSFXVolume(float value)
    {
         float dB = (value > 0) ? Mathf.Log10(value) * 20 : -80;
        musicMixerGroup.audioMixer.SetFloat("KelantanganSFX", dB);
    }

    public void SetAmbientVolume(float value)
    {
         float dB = (value > 0) ? Mathf.Log10(value) * 20 : -80;
        musicMixerGroup.audioMixer.SetFloat("KelantanganAmbient", dB);
    }
}
