using System;
using UnityEngine;

/// <summary>
/// InGameScene에서 사용할 이벤트 버스
/// </summary>
public class UI_InGameEventBus : MonoBehaviour
{
    public static Action OnToggleChoiceRoomCanvas;
    public static Action<float> OnHpSliderValueUpdate;

    public static Action<int> OnActiveMinimapRoom;
    public static Action<int> OnDeactivateMinimapRoom;

    public static void Clear()
    {
        OnToggleChoiceRoomCanvas = null;
        OnHpSliderValueUpdate = null;

        OnActiveMinimapRoom = null;
        OnDeactivateMinimapRoom = null;
    }
}