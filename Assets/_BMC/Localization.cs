using UnityEngine;
using UnityEngine.Localization.Settings;

public class Localization : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            UpdateLocales(0);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            // 현재 로케일을 한국어로 변경
            UpdateLocales(1);
            //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("ko");
        }
    }

    /// <summary>
    /// 0: 영어, 1: 한국어
    /// </summary>
    /// <param name="index"></param>
    public void UpdateLocales(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}