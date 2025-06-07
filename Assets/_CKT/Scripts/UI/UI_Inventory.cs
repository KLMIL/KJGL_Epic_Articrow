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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _canvas.enabled = !_canvas.enabled;
            }
        }
    }
}