using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public enum SliderType
    {
        MusicVolume,
        SFXVolume,
    }

    public class SettingsSlider : MonoBehaviour
    {
        Slider _slider;
        [SerializeField] SliderType _sliderType;

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        void Start()
        {
            _slider.value = _sliderType == SliderType.MusicVolume ? PlayerPrefs.GetFloat("MusicVolume", 1.0f) : PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            OnValueChanged(_slider.value);
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        void OnValueChanged(float value)
        {
            if(_sliderType == SliderType.MusicVolume)
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
                YSJ.Managers.Sound.UpdateBGMVolume(value);
            }
            else if (_sliderType == SliderType.SFXVolume)
            {
                PlayerPrefs.SetFloat("SFXVolume", value);
                YSJ.Managers.Sound.UpdateSFXVolume(value);
            }
        }
    }
}