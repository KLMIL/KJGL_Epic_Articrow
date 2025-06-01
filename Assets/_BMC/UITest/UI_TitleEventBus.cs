using System;
using UnityEngine;

public class UI_TitleEventBus : MonoBehaviour
{
    public static Action<bool> OnToggleMainCanvas;        // 메인 캔버스 토글
    public static Action OnToggleSettingsCanvas;          // 설정 캔버스 토글
    public static Action<int> OnActivePanel;                  // 패널 활성화
    public static Action<int> OnActivePanelBtnHighlightLine;  // 패널 버튼 하이라이트 라인 활성화

    public static void Clear()
    {
        OnToggleMainCanvas = null;
        OnToggleSettingsCanvas = null;
        OnActivePanel = null;
        OnActivePanelBtnHighlightLine = null;
    }
}