using UnityEngine;
using UnityEngine.UI;

public class SettingsBtn : MonoBehaviour
{
    Button _btn;

    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        UI_TitleEventBus.OnToggleSettingsCanvas?.Invoke();
        UI_TitleEventBus.OnActivePanel?.Invoke(0);
        UI_TitleEventBus.OnActivePanelBtnHighlightLine?.Invoke(0);
    }
}