using UnityEngine;

public class KeyConfirmationCanvas : MonoBehaviour
{
    Canvas _canvas;
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        //UI_TitleEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
        UI_CommonEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
    }

    void ToggleCanvas(bool isActive)
    {
        _canvas.enabled = isActive;
    }
}