using UnityEngine;

namespace BMC
{
    public class UI_ChoiceRoomCanvas : MonoBehaviour
    {
        Canvas _canvas;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            UI_EventBus.OnToggleChoiceRoomCanvas += ToggleCanvas;
        }

        void Start()
        {
            _canvas.enabled = false; // 초기에는 캔버스를 비활성화
        }

        void ToggleCanvas()
        {
            Debug.Log("캔버스");
            _canvas.enabled = !_canvas.enabled;
        }

        void OnDestroy()
        {
            UI_EventBus.OnToggleChoiceRoomCanvas -= ToggleCanvas;
        }
    }
}