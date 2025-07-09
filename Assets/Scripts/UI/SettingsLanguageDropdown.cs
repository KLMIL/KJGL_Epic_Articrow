using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using YSJ;

public class SettingsLanguageDropdown : MonoBehaviour
{
    public static Action AffectLocalizationSettings; // 언어 설정 변경 시, 영향 받아야 할 것들에게 알림을 보내기 위한 액션
    
    public TMP_Dropdown LanguageDropdown;

    void Awake()
    {
        AffectLocalizationSettings = null;
        LanguageDropdown = GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        // 언어 설정 불러오기
        int localeIndex = Managers.Data.LocaleIndex;

        // 불러온 언어 인덱스 적용
        if (localeIndex < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0]; // 기본값으로 첫 번째 언어 설정
        }

        // 드롭다운 초기화
        LanguageDropdown.ClearOptions();

        // 언어 옵션 추가
        List<string> options = new List<string>();
        options.Add("English");     // 영어
        options.Add("한국어");       // 한국어
        LanguageDropdown.AddOptions(options);   // 드롭다운에 추가
        LanguageDropdown.value = localeIndex;
        LanguageDropdown.RefreshShownValue();

        // 옵션 선택 시 언어 설정
        LanguageDropdown.onValueChanged.AddListener(delegate
        {
            SetLocales(LanguageDropdown.value);
            AffectLocalizationSettings?.Invoke(); // 언어 설정 변경 알림
        });
    }

    public void SetLocales(int index)
    {
        // 선택한 인덱스의 언어로 설정
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        // 언어 설정 저장
        Managers.Data.SaveLocaleIndex(index);
    }
}