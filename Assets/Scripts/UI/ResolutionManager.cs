using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    List<Resolution> _resolutionList = new List<Resolution>();   // 해상도 리스트
    int _optimalResolutionIndex = 0;                            // 가장 적합한 해상도 인덱스

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _resolutionList.Add(new Resolution { width = 1280, height = 720 });
        _resolutionList.Add(new Resolution { width = 1280, height = 800 });
        _resolutionList.Add(new Resolution { width = 1440, height = 900 });
        _resolutionList.Add(new Resolution { width = 1600, height = 900 });
        _resolutionList.Add(new Resolution { width = 1680, height = 1050 });
        _resolutionList.Add(new Resolution { width = 1920, height = 1080 });
        _resolutionList.Add(new Resolution { width = 1920, height = 1200 });
        _resolutionList.Add(new Resolution { width = 2048, height = 1280 });
        _resolutionList.Add(new Resolution { width = 2560, height = 1440 });
        _resolutionList.Add(new Resolution { width = 2560, height = 1600 });
        _resolutionList.Add(new Resolution { width = 2880, height = 1800 });
        _resolutionList.Add(new Resolution { width = 3480, height = 2160 });

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < _resolutionList.Count; i++)
        {
            string option = _resolutionList[i].width + " x " + _resolutionList[i].height;

            // 가장 적합한 해상도에 별표를 표기
            if (_resolutionList[i].width == Screen.currentResolution.width && _resolutionList[i].height == Screen.currentResolution.height)
            {
                _optimalResolutionIndex = i;
                option += " *";
            }
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = _optimalResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(delegate {
            SetResolution(resolutionDropdown.value);
        });

        // 가장 적합한 해상도로 시작되도록 설정
        SetResolution(_optimalResolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.LogWarning(resolutionIndex + " : " + resolution.width + " x " + resolution.height);
    }
}