using UnityEngine;
using UnityEngine.UI;
using YSJ;

namespace BMC
{
    // 설정 창 닫기 버튼
    public class SettingsCloseBtn : MonoBehaviour
    {
        Button _btn;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);
            Managers.UI.SettingsCloseBtn = this; // UIManager에 등록
        }

        void OnClick()
        {
            CloseSettingsCanvas();
        }

        public void CloseSettingsCanvas()
        {
            UI_CommonEventBus.OnToggleSettingsCanvas?.Invoke(false);
            UI_CommonEventBus.OnDeactivatePanelCanvas?.Invoke();
        }
    }
}