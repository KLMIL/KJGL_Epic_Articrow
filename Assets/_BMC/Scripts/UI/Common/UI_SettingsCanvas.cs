using UnityEngine;
using YSJ;

public class UI_SettingsCanvas : MonoBehaviour
{
    public Canvas canvas;
    CanvasGroup _canvasGroup;
    int _sortingOrder;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        _sortingOrder = canvas.sortingOrder;
        _canvasGroup = GetComponent<CanvasGroup>();
        //UI_TitleEventBus.OnToggleSettingsCanvas = ToggleCanvas;
        UI_CommonEventBus.OnToggleSettingsCanvas = ToggleCanvas;
        Managers.UI.SettingsCanvas = this; // UIManager에 등록
    }

    void ToggleCanvas(bool isActive)
    {
        canvas.enabled = isActive;
        _canvasGroup.interactable = isActive;
        canvas.sortingOrder = (canvas.enabled) ? _sortingOrder : -1; // 인게임에서 다른 UI 가리는 문제를 해결하기 위한 부분
    }
}