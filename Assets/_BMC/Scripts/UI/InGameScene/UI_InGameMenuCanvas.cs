using UnityEngine;
using YSJ;

namespace BMC
{
    public class UI_InGameMenuCanvas : MonoBehaviour
    {
        Canvas _canvas;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        void Start()
        {
            Managers.Input.OnPauseAction = ToggleInGameMenuCanvas;
        }

        void ToggleInGameMenuCanvas()
        {
            _canvas.enabled = !_canvas.enabled;
            GameManager.Instance.TogglePauseGame();
        }
    }
}