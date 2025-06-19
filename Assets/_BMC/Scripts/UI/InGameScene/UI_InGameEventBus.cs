using System;
using UnityEngine;

/// <summary>
/// InGameScene에서 사용할 이벤트 버스
/// </summary>
public class UI_InGameEventBus : MonoBehaviour
{
    // 방 선택
    public static Action OnToggleChoiceRoomCanvas;

    // 피해
    public static Action OnShowBloodCanvas;

    // 플레이어, 보스 HP 슬라이더 업데이트
    public static Action<float> OnPlayerHpSliderValueUpdate;    // 플레이어 HP 슬라이더 값 업데이트
    public static Action<float> OnPlayerHpSliderMaxValueUpdate; // 플레이어 HP 슬라이더 최대값 업데이트
    public static Action<float> OnPlayerMpSliderValueUpdate;    // 플레이어 MP 슬라이더 값 업데이트
    public static Action<float> OnPlayerMpSliderMaxValueUpdate; // 플레이어 MP 슬라이더 최대값 업데이트
    public static Action<float> OnBossHpSliderValueUpdate;      // 보스 HP 슬라이더 값 업데이트

    // 미니맵
    public static Action<int> OnActiveMinimapRoom;     // 미니맵 방 활성화
    public static Action<int> OnDeactivateMinimapRoom; // 미니맵 방 비활성화

    public static void Clear()
    {
        OnToggleChoiceRoomCanvas = null;
        OnShowBloodCanvas = null;
        OnPlayerHpSliderValueUpdate = null;
        OnPlayerHpSliderMaxValueUpdate = null;
        OnPlayerMpSliderValueUpdate = null;
        OnPlayerMpSliderMaxValueUpdate = null;
        OnBossHpSliderValueUpdate = null;
        OnActiveMinimapRoom = null;
        OnDeactivateMinimapRoom = null;
    }
}