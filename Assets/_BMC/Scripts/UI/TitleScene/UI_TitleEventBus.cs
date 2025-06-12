using System;
using UnityEngine;

/// <summary>
/// TitleScene에서 사용할 UI 이벤트 버스
/// </summary>
public class UI_TitleEventBus : MonoBehaviour
{
    public static Action<bool> OnToggleMainBtnCanvas;            // 메인 캔버스 토글

    public static void Clear()
    {
        OnToggleMainBtnCanvas = null;
    }
}