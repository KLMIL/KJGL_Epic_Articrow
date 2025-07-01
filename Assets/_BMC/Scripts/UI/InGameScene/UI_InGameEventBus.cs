using System;
using UnityEngine;

/// <summary>
/// InGameScene에서 사용할 이벤트 버스
/// </summary>
public class UI_InGameEventBus : MonoBehaviour
{
    // 피해
    public static Action OnShowBloodCanvas;

    // 게임 오버
    public static Action OnShowGameOverCanvas;                  // 게임 오버 화면 표시

    // 플레이어, 보스 HP 슬라이더
    public static Action OnPlayerHeartUpdate;                   // 플레이어 하트 업데이트
    public static Action OnPlayerManaUpdate;                    // 플레이어 하트 업데이트
    //public static Action<float> OnPlayerHpSliderValueUpdate;            // 플레이어 HP 슬라이더 값 업데이트
    //public static Action<float> OnPlayerHpSliderMaxValueUpdate;         // 플레이어 HP 슬라이더 최대값 업데이트
    public static Action<float> OnPlayerDashCoolTimeSliderValueUpdate;  // 플레이어 대시 쿨타임 슬라이더 값 업데이트
    public static Action<float> OnPlayerDashCoolTimeMaxValueUpdate;     // 플레이어 대시 쿨타임 슬라이더 최대값 업데이트
    public static Action<float> OnPlayerMpSliderValueUpdate;            // 플레이어 MP 슬라이더 값 업데이트
    public static Action<float> OnPlayerMpSliderMaxValueUpdate;         // 플레이어 MP 슬라이더 최대값 업데이트
    public static Action<float> OnBossHpSliderValueUpdate;              // 보스 HP 슬라이더 값 업데이트

    public static void Clear()
    {
        OnPlayerHeartUpdate = null;
        OnPlayerManaUpdate = null;
        OnShowBloodCanvas = null;
        //OnPlayerHpSliderValueUpdate = null;
        //OnPlayerHpSliderMaxValueUpdate = null;
        OnPlayerDashCoolTimeSliderValueUpdate = null;
        OnPlayerDashCoolTimeMaxValueUpdate = null;
        OnPlayerMpSliderValueUpdate = null;
        OnPlayerMpSliderMaxValueUpdate = null;
        OnBossHpSliderValueUpdate = null;
    }
}