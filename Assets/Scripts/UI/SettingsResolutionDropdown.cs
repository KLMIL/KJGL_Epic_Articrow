using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 해상도 설정 드롭다운
/// </summary>
public class SettingsResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        resolutions = Screen.resolutions;           // 모니터가 전체 화면으로 지원하는 모든 해상도 불러오기
        ResolutionDropdown.ClearOptions();          // 드롭다운 초기화
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i=0; i<resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options); // 드롭다운에 추가
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        // 옵션 선택 시 해상도 설정
        ResolutionDropdown.onValueChanged.AddListener(delegate
        {
            SetResolution(ResolutionDropdown.value);
        });
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.LogWarning(resolutionIndex + " : " + resolution.width + " x " + resolution.height);
    }
}
