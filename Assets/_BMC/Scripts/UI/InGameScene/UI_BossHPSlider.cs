using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class UI_BossHPSlider : UI_StatusSlider
    {
        Canvas _canvas;

        void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        void Start()
        {
            UI_InGameEventBus.OnToggleBossHpSlider = Toggle;
            UI_InGameEventBus.OnBossHpSliderValueUpdate = base.UpdateSlider;
            BossStatus bossStatus = transform.parent.GetComponentInParent<BossStatus>();
            base.Init(bossStatus.Health);
        }

        public void Toggle(bool isActive)
        {
            _canvas.enabled = isActive;
        }
    }
}