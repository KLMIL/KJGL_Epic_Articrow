using UnityEngine;

namespace CKT
{
    public class UI_EquipcParts : MonoBehaviour
    {
        Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
        }

        public void CanvasEnable(bool value)
        {
            _canvas.enabled = value;

            GameManager.Instance.TogglePauseGame(value);
        }
    }
}