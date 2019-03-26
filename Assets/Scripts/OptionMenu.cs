using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
    }

    public void OnMusicVolumeChange(float _newVol)
    {
        print("Music volume changed");
        audioMixer.SetFloat("MusicVol", _newVol);
    }

    public void OnSfxVolumeChange(float _newVol)
    {
        audioMixer.SetFloat("SfxVol", _newVol);
    }
}