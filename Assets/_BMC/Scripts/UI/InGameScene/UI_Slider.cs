using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    // TODO: 현재 Hp만 존재하므로 Hp 보여주는 슬라이더로 사용중, 나중에 일반화하여 사용할 수 있도록 개선 필요
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