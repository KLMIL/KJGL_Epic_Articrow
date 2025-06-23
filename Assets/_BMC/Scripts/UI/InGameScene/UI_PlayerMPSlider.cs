using UnityEngine;
using YSJ;

namespace BMC
{
    public class UI_PlayerMPSlider : UI_StatusSlider
    {
        void Start()
        {
            UI_InGameEventBus.PlayerStatusCanvas = GetComponentInParent<Canvas>();
            UI_InGameEventBus.ManaSliderHandle = transform.Find("HandleSlideArea/Handle").GetComponent<RectTransform>();
            UI_InGameEventBus.OnPlayerMpSliderMaxValueUpdate = base.SetMaxValue;
            UI_InGameEventBus.OnPlayerMpSliderValueUpdate = base.UpdateSlider;
            PlayerStatus playerStatus = PlayerManager.Instance.PlayerStatus;
            base.Init(playerStatus.MaxMana);
        }
    }
}