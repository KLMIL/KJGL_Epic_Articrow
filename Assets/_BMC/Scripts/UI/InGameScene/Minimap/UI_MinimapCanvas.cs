using UnityEngine;
using YSJ;

namespace BMC
{
    public class UI_MinimapCanvas : MonoBehaviour
    {
        Canvas _canvas;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        void Start()
        {
            Managers.Input.OnInventoryAction += ToggleCanvas;
        }

        void ToggleCanvas()
        {
            _canvas.enabled = !_canvas.enabled;
        }

        private void OnDestroy()
        {
            Managers.Input.OnInventoryAction -= ToggleCanvas;
        }
    }
}