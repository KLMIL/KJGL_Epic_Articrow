using UnityEngine;
using YSJ;

public class KeyConfirmationCanvas : MonoBehaviour
{
    public Canvas Canvas;
    void Awake()
    {
        Canvas = GetComponent<Canvas>();
        //UI_TitleEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
        UI_CommonEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
        Managers.UI.KeyConfirmationCanvas = this;
    }

    void ToggleCanvas(bool isActive)
    {
        Canvas.enabled = isActive;
    }
}