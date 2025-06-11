using System;
using UnityEngine;

public class UI_CommonEventBus : MonoBehaviour
{
    public static Action OnToggleSettingsCanvas;              // 설정 캔버스 토글
    public static Action<int> OnActivePanelCanvas;            // 패널 활성화
    public static Action<int> OnActivePanelBtnHighlightLine;  // 패널 버튼 하이라이트 라인 활성화
    public static Action<bool> OnActiveKeyConfirmationCanvas; // 키 확정 캔버스 토글
    public static Action OnResetKeyBind;                      // 키 바인딩 초기화

    public static void Clear()
    {
        OnToggleSettingsCanvas = null;
        OnActivePanelCanvas = null;
        OnActivePanelBtnHighlightLine = null;
        OnActiveKeyConfirmationCanvas = null;
        OnResetKeyBind = null;
    }
}