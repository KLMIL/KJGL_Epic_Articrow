using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 해상도 설정 드롭다운
/// </summary>
public class SettingsResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    Resolution[] resolutions;
    List<Resolution> _filteredResolutions = new List<Resolution>();

    void Awake()
    {
        ResolutionDropdown = GetComponent<TMP_Dropdown>();
        Init();
    }

    public void Init()
    {
        resolutions = Screen.resolutions;           // 모니터가 전체 화면으로 지원하는 모든 해상도 불러오기
        ResolutionDropdown.ClearOptions();          // 드롭다운 초기화

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            //string option = resolutions[i].width + " x " + resolutions[i].height;
            //options.Add(option);

            //if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            //{
            //    currentResolutionIndex = i;
            //}

            // 16:9 비율의 해상도만 구하기
            float aspect = (float)resolutions[i].width / resolutions[i].height;
            if (Mathf.Abs(aspect - (16f / 9f)) < 0.01f)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;

                if (!options.Contains(option)) // 중복 방지
                {
                    _filteredResolutions.Add(resolutions[i]); // 여기서 따로 저장
                    options.Add(option);

                    // 여기서도 _filteredResolutions.Count 기준으로 비교
                    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = _filteredResolutions.Count - 1;
                    }
                }
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
        if (resolutionIndex < 0 || resolutionIndex >= _filteredResolutions.Count)
            return;

        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.LogWarning(resolutionIndex + " : " + resolution.width + " x " + resolution.height);

        // 해상도 인덱스 저장
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }
    //public TMP_Dropdown ResolutionDropdown;
    //Resolution[] resolutions;

    //int _optionalWidth = 1920;   // 최적 너비
    //int _optionalHeight = 1080;  // 최적 높이

    //public static Rect CameraRect { get; private set; }

    //void Awake()
    //{
    //    Init();
    //}

    //public void Init()
    //{
    //    resolutions = Screen.resolutions;           // 모니터가 전체 화면으로 지원하는 모든 해상도 불러오기
    //    ResolutionDropdown.ClearOptions();          // 드롭다운 초기화

    //    List<string> options = new List<string>();
    //    int currentResolutionIndex = 0;
    //    for(int i=0; i<resolutions.Length; i++)
    //    {
    //        // 16:9 비율의 해상도만 구하기
    //        float aspect = (float)resolutions[i].width / resolutions[i].height;
    //        if(Mathf.Abs(aspect - (16f/9f)) < 0.01f)
    //        {
    //            string option = resolutions[i].width + " x " + resolutions[i].height;
    //            options.Add(option);

    //            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
    //            {
    //                currentResolutionIndex = i;
    //            }
    //        }
    //    }
    //    ResolutionDropdown.AddOptions(options); // 드롭다운에 추가

    //    // 저장된 해상도 인덱스를 불러와서 설정
    //    int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);
    //    savedIndex = Mathf.Clamp(savedIndex, 0, resolutions.Length - 1);
    //    ResolutionDropdown.value = savedIndex;
    //    ResolutionDropdown.RefreshShownValue();
    //    SetResolution(ResolutionDropdown.value);

    //    // 옵션 선택 시 해상도 설정 이벤트 등록
    //    ResolutionDropdown.onValueChanged.AddListener(SetResolution);
    //}

    //public void SetResolution(int resolutionIndex)
    //{
    //    if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
    //        return;

    //    //Resolution resolution = resolutions[resolutionIndex];
    //    //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //    //Debug.LogWarning(resolutionIndex + " : " + resolution.width + " x " + resolution.height);

    //    /*------*/

    //    int deviceWidth = resolutions[resolutionIndex].width; // 기기 너비 저장
    //    int deviceHeight = resolutions[resolutionIndex].height; // 기기 높이 저장

    //    //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), Screen.fullScreen); // SetResolution 함수 제대로 사용하기

    //    Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen); // SetResolution 함수 제대로 사용하기

    //    if ((float)_optionalWidth / _optionalHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
    //    {
    //        float newWidth = ((float)_optionalWidth / _optionalHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
    //        CameraRect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
    //        //Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
    //    }
    //    else // 게임의 해상도 비가 더 큰 경우
    //    {
    //        float newHeight = ((float)deviceWidth / deviceHeight) / ((float)_optionalWidth / _optionalHeight); // 새로운 높이
    //        CameraRect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
    //        //Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
    //    }
    //    Camera.main.rect = CameraRect; // 새로운 Rect 적용

    //    // 해상도 인덱스 저장
    //    PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    //    PlayerPrefs.Save();
    //}
}