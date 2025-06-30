using UnityEngine;

namespace CKT
{
    public class UI_Inventory : MonoBehaviour
    {
        Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            YSJ.Managers.Input.OnInventoryAction += UI_CanvasEnable;
        }

        private void OnDisable()
        {
            YSJ.Managers.Input.OnInventoryAction -= UI_CanvasEnable;
        }

        void UI_CanvasEnable()
        {
            _canvas.enabled = !_canvas.enabled;
        }
    }
}