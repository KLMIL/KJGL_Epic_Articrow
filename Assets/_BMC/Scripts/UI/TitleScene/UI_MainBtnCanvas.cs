using UnityEngine;

public class UI_MainBtnCanvas : MonoBehaviour
{
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UI_TitleEventBus.OnToggleMainBtnCanvas += ToggleMainCanvas;
    }

    void ToggleMainCanvas(bool isActive)
    {
        _canvas.enabled = isActive;
    }
}