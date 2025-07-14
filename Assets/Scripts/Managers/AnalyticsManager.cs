using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using BMC;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;

    bool isGameStart = false;

    public AnalyticsData analyticsData = new AnalyticsData(); // 통계 분석 데이터 클래스
    Stopwatch _stopWatch;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void InitData()
    {
        analyticsData.Init();
    }

    // 초기화
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    #region 타이머 관련
    // 타이머 시작
    public void StartTimeCounter()
    {
        if (_stopWatch == null)
            _stopWatch = new Stopwatch();

        if (!_stopWatch.IsRunning)
        {
            _stopWatch.Start();
            isGameStart = true;
        }
    }

    // 타이머 정지
    public void StopTimeCounter()
    {
        if (_stopWatch == null)
        {
            return;
        }

        _stopWatch.Stop();
        int time = (int)_stopWatch.Elapsed.TotalSeconds;
        analyticsData.playTime = time;
    }
    #endregion

    // 통계 시작
    public void StartAnalystics()
    {
        //return; // QA를 위한 연결 해제

        if (!_isInitialized)
        {
            return;
        }

        InitData(); // 데이터 초기화
        StartTimeCounter(); // 타이머 시작
        Debug.Log("통계 분석 시작");
    }

    // 통계 전송
    public void SendAnalytics()
    {
        //return; // QA를 위한 연결 해제

        if (!_isInitialized || !isGameStart)
        {
            return;
        }

        // 보낼 때 스테이지 기록
        analyticsData.progressStage = GameFlowManager.Instance.CurrentRoom;

        // 인게임 중인 경우, 장착한 파츠 개수
        PlayerManager.Instance.PlayerHand.RightHand.GetComponentInChildren<Artifact_YSJ>().CountEquipParts();
        
        // 타이머 정지
        StopTimeCounter();

        // 이벤트 생성
        CustomEvent analyticsEvent = new CustomEvent("playAnalytics")
        {
            { "countArtifactSwap", analyticsData.countArtifactSwap },
            { "playerHurtCount", analyticsData.playerHurtCount },
            { "normalAttackCount", analyticsData.normalAttackCount },
            { "skillAttackCount", analyticsData.skillAttackCount },
            { "isPlayerDead", analyticsData.isPlayerDead },
            { "currentEquipPartsCount", analyticsData.currentEquipPartsCount },
            { "playTime", analyticsData.playTime },
            { "progressStage", analyticsData.progressStage },
        };

        AnalyticsService.Instance.RecordEvent(analyticsEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("통계 분석 전송");

        isGameStart = false;
    }
}