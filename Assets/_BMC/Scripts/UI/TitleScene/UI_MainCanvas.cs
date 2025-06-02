using UnityEngine;

public class UI_MainCanvas : MonoBehaviour
{
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UI_TitleEventBus.OnToggleMainCanvas += ToggleMainCanvas;
    }

    void ToggleMainCanvas(bool isActive)
    {
        _canvas.enabled = isActive;
    }
}