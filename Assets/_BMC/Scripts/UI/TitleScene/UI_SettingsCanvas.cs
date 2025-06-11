using UnityEngine;

public class UI_SettingsCanvas : MonoBehaviour
{
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        //UI_TitleEventBus.OnToggleSettingsCanvas = ToggleCanvas;
        UI_CommonEventBus.OnToggleSettingsCanvas = ToggleCanvas;
    }

    void ToggleCanvas()
    {
        _canvas.enabled = !_canvas.enabled;
        _canvas.sortingOrder = (_canvas.enabled) ? 2 : -1; // 인게임에서 다른 UI 가리는 문제를 해결하기 위한 부분
    }
}