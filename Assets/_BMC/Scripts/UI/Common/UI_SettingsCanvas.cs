using UnityEngine;

public class UI_SettingsCanvas : MonoBehaviour
{
    Canvas _canvas;
    CanvasGroup _canvasGroup;
    int _sortingOrder;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _sortingOrder = _canvas.sortingOrder;
        _canvasGroup = GetComponent<CanvasGroup>();
        //UI_TitleEventBus.OnToggleSettingsCanvas = ToggleCanvas;
        UI_CommonEventBus.OnToggleSettingsCanvas = ToggleCanvas;
    }

    void ToggleCanvas(bool isActive)
    {
        _canvas.enabled = isActive;
        _canvasGroup.interactable = isActive;
        _canvas.sortingOrder = (_canvas.enabled) ? _sortingOrder : -1; // 인게임에서 다른 UI 가리는 문제를 해결하기 위한 부분
    }
}