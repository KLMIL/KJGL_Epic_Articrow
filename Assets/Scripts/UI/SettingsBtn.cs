using UnityEngine;
using UnityEngine.UI;

public class SettingsBtn : MonoBehaviour
{
    Button _btn;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        UI_TitleEventBus.OnToggleMainBtnCanvas?.Invoke(false);
        //UI_TitleEventBus.OnToggleSettingsCanvas?.Invoke();
        //UI_TitleEventBus.OnActivePanelCanvas?.Invoke(0);
        //UI_TitleEventBus.OnActivePanelBtnHighlightLine?.Invoke(0);
        UI_CommonEventBus.OnToggleSettingsCanvas?.Invoke();
        UI_CommonEventBus.OnActivePanelCanvas?.Invoke(0);
        UI_CommonEventBus.OnActivePanelBtnHighlightLine?.Invoke(0);
    }
}