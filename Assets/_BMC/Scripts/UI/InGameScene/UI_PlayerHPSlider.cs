using YSJ;

namespace BMC
{
    public class UI_PlayerHPSlider : UI_StatusSlider
    {
        void Start()
        {
            UI_InGameEventBus.OnPlayerHpSliderValueUpdate = base.UpdateSlider;
            PlayerStatus playerStatus = PlayerManager.Instance.PlayerStatus;
            base.Init(playerStatus.MaxHealth);
        }
    }
}