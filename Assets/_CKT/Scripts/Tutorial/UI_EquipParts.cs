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

            TutorialManager.Instance.OnEquipPartsActionT1.SingleRegister((value) => CanvasEnable(value));
            Debug.Log("[ckt] UI_EquipParts Start");
        }

        public void CanvasEnable(bool value)
        {
            _canvas.enabled = value;
            GameManager.Instance.TogglePauseGame(value);
            Debug.Log($"[ckt] UI_EquipParts CanvasEnable {value}");
        }
    }
}