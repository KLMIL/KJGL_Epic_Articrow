using System;
using UnityEngine;

/// <summary>
/// InGameScene에서 사용할 이벤트 버스
/// </summary>
public class UI_InGameEventBus : MonoBehaviour
{
    // 방 선택
    public static Action OnToggleChoiceRoomCanvas;

    // 플레이어, 보스 HP 슬라이더 업데이트
    public static Action<float> OnPlayerHpSliderValueUpdate;
    public static Action<float> OnPlayerMpSliderValueUpdate;
    public static Action<float> OnBossHpSliderValueUpdate;

    // 미니맵
    public static Action<int> OnActiveMinimapRoom;
    public static Action<int> OnDeactivateMinimapRoom;

    public static void Clear()
    {
        OnToggleChoiceRoomCanvas = null;
        OnPlayerHpSliderValueUpdate = null;
        OnBossHpSliderValueUpdate = null;
        OnActiveMinimapRoom = null;
        OnDeactivateMinimapRoom = null;
    }
}