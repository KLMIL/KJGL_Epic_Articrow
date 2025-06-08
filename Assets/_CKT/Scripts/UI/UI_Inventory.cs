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
            YSJ.Managers.Input.OnInventoryAction += UI_EnableSwitch;
        }

        void UI_EnableSwitch()
        {
            _canvas.enabled = !_canvas.enabled;
        }
    }
}