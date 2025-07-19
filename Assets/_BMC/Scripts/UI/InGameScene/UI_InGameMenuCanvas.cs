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
            // 설정 창이 열려 있으면 설정 창 닫고 종료
            if (Managers.UI.SettingsCanvas.canvas.enabled)
            {
                Managers.UI.SettingsCanvas.canvas.enabled = false;
                UI_CommonEventBus.OnToggleSettingsCanvas?.Invoke(false);
                return;
            }

            _canvas.enabled = !_canvas.enabled;

            // 일시 정지 캔버스 Off
            if (!_canvas.enabled)
            {
                GameManager.Instance.TogglePauseGame(false);
            }
            // 일시 정지 캔버스 On, 설정 캔버스 Off 되어 있는 경우
            else if(_canvas.enabled && !Managers.UI.SettingsCanvas.canvas.enabled)
            {
                // 게임 일시 정지
                GameManager.Instance.TogglePauseGame(true);
            }
        }
    }
}