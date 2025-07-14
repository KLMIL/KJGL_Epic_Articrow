using UnityEngine;
using UnityEngine.UI;

public class GameExitBtn : MonoBehaviour
{
    Button _btn;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        // 빌드할 때는 Editor 부분 지우기
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //AnalyticsManager.Instance.analyticsData.progressStage = GameFlowManager.Instance.CurrentRoom;
        //AnalyticsManager.Instance.SendAnalytics(); // 게임 종료 시 통계 데이터 전송
#else
                    AnalyticsManager.Instance.analyticsData.progressStage = GameFlowManager.Instance.CurrentRoom; // 현재 진행 중인 스테이지 저장   
                    AnalyticsManager.Instance.analyticsData.SendAnalytics(); // 게임 종료 시 통계 데이터 전송
                    Application.Quit();
#endif
    }
}