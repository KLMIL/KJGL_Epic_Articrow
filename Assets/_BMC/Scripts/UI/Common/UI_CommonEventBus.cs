using System;
using UnityEngine;

/// <summary>
/// 여러 씬에서 사용할 UI 이벤트 버스
/// </summary>
public class UI_CommonEventBus : MonoBehaviour
{
    public static Action<bool> OnToggleSettingsCanvas;        // 설정 캔버스 토글
    public static Action<int> OnActivePanelCanvas;            // 패널 활성화
    public static Action OnDeactivatePanelCanvas;             // 패널 비활성화
    public static Action<int> OnActivePanelBtnHighlightLine;  // 패널 버튼 하이라이트 라인 활성화
    public static Action<bool> OnActiveKeyConfirmationCanvas; // 키 확정 캔버스 토글
    public static Action OnResetKeyBind;                      // 키 바인딩 초기화

    public static void Clear()
    {
        OnToggleSettingsCanvas = null;
        OnActivePanelCanvas = null;
        OnDeactivatePanelCanvas = null;
        OnActivePanelBtnHighlightLine = null;
        OnActiveKeyConfirmationCanvas = null;
        OnResetKeyBind = null;
    }
}