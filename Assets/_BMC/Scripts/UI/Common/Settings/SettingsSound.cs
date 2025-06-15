using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsSound : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicMasterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        _musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        _bgmSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
