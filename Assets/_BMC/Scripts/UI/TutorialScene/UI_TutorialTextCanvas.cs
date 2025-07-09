using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace BMC
{
    public class UI_TutorialTextCanvas : MonoBehaviour
    {
        #region Localization
        static readonly string TutorialTable = "TutorialTable";
        string _key;
        #endregion

        TextMeshProUGUI _guideText;

        void Awake()
        {
            _guideText = GetComponentInChildren<TextMeshProUGUI>();
            UI_TutorialEventBus.OnTutorialText = SetText;
        }

        void Start()
        {
            SettingsLanguageDropdown.AffectLocalizationSettings += UpdateLocalization;
        }

        public void SetText(int id = -1)
        {
            if(id == -1)
            {
                _guideText.text = string.Empty;
            }

            // 로컬라이제이션 설정에 따라 튜토리얼 텍스트 설정
            _key = id.ToString();
            string tutorialText = LocalizationSettings.StringDatabase.GetLocalizedString(TutorialTable, _key, LocalizationSettings.SelectedLocale);
            _guideText.text = tutorialText;
        }

        // 언어 설정 시, 업데이트
        public void UpdateLocalization()
        {
            string tutorialText = LocalizationSettings.StringDatabase.GetLocalizedString(TutorialTable, _key, LocalizationSettings.SelectedLocale);
            _guideText.text = tutorialText;
        }

        void OnDestroy()
        {
            SettingsLanguageDropdown.AffectLocalizationSettings -= UpdateLocalization;
            UI_TutorialEventBus.OnTutorialText -= SetText;
        }
    }
}