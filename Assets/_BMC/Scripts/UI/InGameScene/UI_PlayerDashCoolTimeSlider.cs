using BMC;
using YSJ;

namespace BMC
{
    public class UI_PlayerDashCoolTimeSlider : UI_StatusSlider
    {
        void Start()
        {
            UI_InGameEventBus.OnPlayerDashCoolTimeMaxValueUpdate = base.SetMaxValue;
            UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate = base.UpdateSlider;
            PlayerDash playerDash = PlayerManager.Instance.PlayerDash;
            base.Init(playerDash.DashCoolTime);
        }
    }
}