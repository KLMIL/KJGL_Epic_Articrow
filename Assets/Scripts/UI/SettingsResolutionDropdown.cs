using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 해상도 설정 드롭다운
/// </summary>
public class SettingsResolutionDropdown : MonoBehaviour
{
    TMP_Dropdown _resolutionDropdown;
    List<Resolution> _filteredResolutions = new List<Resolution>();

    void Awake()
    {
        _resolutionDropdown = GetComponent<TMP_Dropdown>();
        Init();
    }

    public void Init()
    {
        Resolution[] resolutions = Screen.resolutions;  // 모니터가 전체 화면으로 지원하는 모든 해상도 불러오기
        _resolutionDropdown.ClearOptions();             // 드롭다운 초기화

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            // 16:9 비율의 해상도만 구하기
            float aspect = (float)resolutions[i].width / resolutions[i].height;
            if (Mathf.Abs(aspect - (16f / 9f)) < 0.01f)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;

                if (!options.Contains(option)) // 중복 방지
                {
                    options.Add(option);                      // 옵션에 담기 위한 리스트에 추가
                    _filteredResolutions.Add(resolutions[i]); // 해상도 정보 저장

                    // 현재 해상도와 일치하는 경우 인덱스 저장
                    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = _filteredResolutions.Count - 1;
                    }
                }
            }
        }
        _resolutionDropdown.AddOptions(options); // 드롭다운에 추가

        // 저장된 해상도 인덱스를 불러오거나 없으면 선별된 해상도 중 마지막으로 설정
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", _filteredResolutions.Count - 1);
        _resolutionDropdown.value = savedIndex;
        _resolutionDropdown.RefreshShownValue();
        SetResolution(_resolutionDropdown.value);

        // 옵션 선택 시 해상도 설정 이벤트 등록
        _resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= _filteredResolutions.Count)
            return;

        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Debug.LogWarning(resolutionIndex + " : " + resolution.width + " x " + resolution.height);

        // 해상도 인덱스 저장
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }
}