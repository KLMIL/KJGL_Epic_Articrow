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

            // 일시 정지 상태에서 UI 닫을 때, 열려 있는 설정 창도 닫기
            if(!_canvas.enabled )
                UI_CommonEventBus.OnToggleSettingsCanvas?.Invoke(false);

            GameManager.Instance.TogglePauseGame(_canvas.enabled);
        }
    }
}