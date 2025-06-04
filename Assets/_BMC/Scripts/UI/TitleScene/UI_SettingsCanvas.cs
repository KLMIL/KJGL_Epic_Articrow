using Unity.VisualScripting;
using UnityEngine;

public class UI_SettingsCanvas : MonoBehaviour
{
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UI_TitleEventBus.OnToggleSettingsCanvas = ToggleCanvas;
    }

    void ToggleCanvas()
    {
        _canvas.enabled = !_canvas.enabled;
    }
}