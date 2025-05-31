using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class UI_Slider : MonoBehaviour
    {
        Slider _slider;

        void Awake()
        {
            Init(200f);
            UI_InGameEventBus.OnHpSliderValueUpdate = UpdateSlider;
        }

        public void Init(float maxValue)
        {
            _slider = GetComponent<Slider>();
            SetMaxValue(maxValue);
            UpdateSlider(maxValue);
        }

        public void SetMaxValue(float maxValue)
        {
            _slider.maxValue = maxValue;
        }

        public void UpdateSlider(float value)
        {
            _slider.value = value;
        }

        void OnDestroy()
        {
            UI_InGameEventBus.OnHpSliderValueUpdate = null;
        }
    }
}