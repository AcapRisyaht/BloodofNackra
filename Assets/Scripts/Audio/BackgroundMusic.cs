using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
      musicSource.Play(); // mula main bila game start   
    }

    public void StopMusic()
    {
        musicSource.Stop(); // berhenti main bila player mati
    }

    public void ChangeVolume(float volume)
    {
        musicSource.volume = volume; // ubah volume bila player ubah setting
    }

    public void ChangeTrack(AudioClip newTrack)
    {
        musicSource.clip = newTrack; // ganti lagu bila player pilih lagu lain
        musicSource.Play();
    }
}
