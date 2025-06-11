using YSJ;

namespace BMC
{
    public class UI_BossHPSlider : UI_StatusSlider
    {
        void Start()
        {
            UI_InGameEventBus.OnBossHpSliderValueUpdate = base.UpdateSlider;
            BossStatus bossStatus = transform.parent.GetComponentInParent<BossStatus>();
            base.Init(bossStatus.Health);
        }
    }
}