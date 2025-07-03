using UnityEngine;

namespace YSJ
{
    // 데이터를 관리하는 매니저 클래스
    // 게임 데이터 초기화 및 관리 기능 존재 ex) 설정, 도전과제
    public class DataManager
    {
        // 튜토리얼 클리어 여부
        bool _isClearTutorial;
        public bool IsClearTutorial 
        {
            get => _isClearTutorial;
            set
            {
                _isClearTutorial = value;
                PlayerPrefs.SetInt("IsClearTutorial", _isClearTutorial ? 1 : 0);
            }
        }

        public void Init()
        {
            _isClearTutorial = PlayerPrefs.GetInt("IsClearTutorial", 0) == 1;

            // 설정 불러오기
            LoadSettings();
        }

        #region 설정 저장 및 로드
        // 설정 저장
        public void SaveSettings()
        {
            // 설정 파일에 데이터를 저장하는 로직
            // 예: PlayerPrefs, JSON 파일 등에서 설정을 저장하기
            Debug.Log("Settings saved.");
        }

        // 설정 불러오기
        public void LoadSettings()
        {
            // 설정 파일에서 데이터를 불러오는 로직
            // 예: PlayerPrefs, JSON 파일 등에서 설정을 읽어오기
            Debug.Log("Settings loaded.");
        }
        #endregion
    }
}