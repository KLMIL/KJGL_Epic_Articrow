using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace BMC
{
    public class SettingsManager : MonoBehaviour
    {
        static SettingsManager s_instance;
        public static SettingsManager Instance => s_instance;

        public enum Platform { Desktop };
        public Platform platform;
        // toggle buttons

        // TODO UI 자동화 하기 전에 일단 이 방법으로 진행, 추후에 자동화 코드 더 연구해서 할 예정
        Dictionary<OnOffButtonType, SettingsOnOffBtn> _onOffBtnDict = new Dictionary<OnOffButtonType, SettingsOnOffBtn>();
        Dictionary<OnOffButtonType, Action> _onOffBtnActionDict = new Dictionary<OnOffButtonType, Action>();
        public Dictionary<OnOffButtonType, SettingsOnOffBtn> OnOffBtnDict => _onOffBtnDict;
        public Dictionary<OnOffButtonType, Action> OnOffBtnActionDict => _onOffBtnActionDict;

        [Header("비디오 & 오디오 설정")]
        // Language 추가 예정
        public TextMeshProUGUI fullscreentext;  // 전체 화면
        public TextMeshProUGUI vsyncText;       // VSync
        //public SettingsOnOffBtn fullScreenBtn;  // 전체 화면
        //public SettingsOnOffBtn vsyncBtn;       // VSync
        public TMP_Dropdown resolutionDropdown; // 해상도 드롭다운
        public Slider musicSlider;              // 음악 볼륨(BGM)

        [Header("게임 설정")]
        public TextMeshProUGUI tooltipstext;
        public TextMeshProUGUI cameraeffectstext;

        void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            _onOffBtnActionDict.Add(OnOffButtonType.FullScreen, ToggleFullScreen);
            _onOffBtnActionDict.Add(OnOffButtonType.VSync, ToggleVSync);

            // 비디오 & 오디오 설정
            fullscreentext.text = (Screen.fullScreen) ? "O" : "X";
            vsyncText.text = PlayerPrefs.GetInt("VSync") == 1 ? "O" : "X";
            //musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

            // 조작

            // 게임 플레이
            //tooltipstext.text = PlayerPrefs.GetInt("ToolTips") == 1 ? "On" : "Off";
        }

        void Update()
        {
            //sliderValue = musicSlider.GetComponent<Slider>().value;
        }

        #region 비디오 & 오디오 설정
        public void ToggleFullScreen()
        {
            // Screen,fullScreen은 변경되지만, UI 텍스트 업데이트는 그 프레임에서는 반영되지 않고, 다음 프레임에 적용되는 경우가 있을 수 있음
            // 즉, 화면 모드 전환이 일어나면서 Unity가 내부적으로 약간의 지연을 가지기 때문에, Screen.fullScreen을 즉시 읽어오는 것이 정확하지 않을 수 있음
            bool newState = !Screen.fullScreen;
            fullscreentext.text = newState ? "O" : "X";
            Screen.fullScreen = newState;
        }

        public void MusicSlider()
        {
            //PlayerPrefs.SetFloat("MusicVolume", sliderValue);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
        }

        public void ToggleVSync()
        {
            QualitySettings.vSyncCount = (QualitySettings.vSyncCount == 0) ? 1 : 0;
            vsyncText.text = (QualitySettings.vSyncCount == 0) ? "O" : "X";
        }
        #endregion

        #region 조작 설정
        #endregion

        #region 게임 플레이 설정
        // show tool tips like: 'How to Play' control pop ups
        public void ToolTips()
        {
            int toolTipsValue = PlayerPrefs.GetInt("ToolTips");
            int setValue = (toolTipsValue == 0) ? 1 : 0;
            PlayerPrefs.SetInt("ToolTips", setValue);
            tooltipstext.text = (toolTipsValue == 0) ? "O" : "X";
        }

        public void CameraEffects()
        {
            int cameraEffects = PlayerPrefs.GetInt("CameraEffects");
            int setValue = (cameraEffects == 0) ? 1 : 0;
            PlayerPrefs.SetInt("CameraEffects", setValue);
            cameraeffectstext.text = (cameraEffects == 0) ? "O" : "X";
        }
        #endregion
    }
}