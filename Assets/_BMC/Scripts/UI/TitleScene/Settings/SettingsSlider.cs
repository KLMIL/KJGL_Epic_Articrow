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


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnValueChanged(float value)
        {

        }
    }
}