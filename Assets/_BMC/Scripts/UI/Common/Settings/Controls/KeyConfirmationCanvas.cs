using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YSJ;

public class KeyConfirmationCanvas : MonoBehaviour
{
    #region Localization
    static readonly string SettingsControlsTable = "SettingsControlsTable";
    #endregion

    public Canvas Canvas;
    public TextMeshProUGUI GuidText;
    void Awake()
    {
        Canvas = GetComponent<Canvas>();
        GuidText = GetComponentInChildren<TextMeshProUGUI>();
        //UI_TitleEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
        UI_CommonEventBus.OnActiveKeyConfirmationCanvas = ToggleCanvas;
        Managers.UI.KeyConfirmationCanvas = this;
        SetGuidText("PressKey");
    }

    void ToggleCanvas(bool isActive)
    {
        Canvas.enabled = isActive;
    }

    public void SetGuidText(string localizationKey)
    {
        string localeDescription = LocalizationSettings.StringDatabase.GetLocalizedString(SettingsControlsTable, localizationKey, LocalizationSettings.SelectedLocale);
        GuidText.text = localeDescription;
    }
}