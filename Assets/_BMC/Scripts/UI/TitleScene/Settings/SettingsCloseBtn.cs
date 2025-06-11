using UnityEngine;
using UnityEngine.UI;

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
        }

        void OnClick()
        {
            UI_CommonEventBus.OnToggleSettingsCanvas?.Invoke();
        }
    }
}