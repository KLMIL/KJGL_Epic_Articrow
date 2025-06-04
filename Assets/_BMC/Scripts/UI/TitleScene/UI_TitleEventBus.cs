using System;
using UnityEngine;

/// <summary>
/// TitleScene에서 사용할 이벤트 버스
/// </summary>
public class UI_TitleEventBus : MonoBehaviour
{
    public static Action<bool> OnToggleMainBtnCanvas;            // 메인 캔버스 토글
    public static Action OnToggleSettingsCanvas;              // 설정 캔버스 토글
    public static Action<int> OnActivePanelCanvas;            // 패널 활성화
    public static Action<int> OnActivePanelBtnHighlightLine;  // 패널 버튼 하이라이트 라인 활성화
    public static Action<bool> OnActiveKeyConfirmationCanvas; // 키 확정 캔버스 토글
    public static Action OnResetKeyBind;                      // 키 바인딩 초기화

    public static void Clear()
    {
        OnToggleMainBtnCanvas = null;
        OnToggleSettingsCanvas = null;
        OnActivePanelCanvas = null;
        OnActivePanelBtnHighlightLine = null;
        OnActiveKeyConfirmationCanvas = null;
        OnResetKeyBind = null;
    }
}