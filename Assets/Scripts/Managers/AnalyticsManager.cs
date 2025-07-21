using BMC;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using static AnalyticsClass;
using Debug = UnityEngine.Debug;

public class AnalyticsManager : MonoBehaviour
{
    public bool isActive;

    public static AnalyticsManager Instance;
    private bool _isInitialized = false;

    bool isGameStart = false;

    public AllPlayerHurtInfo allPlayerHurtInfo = new AllPlayerHurtInfo(); // 방 이름과 해당 방에서 플레이어에게 피해를 준 공격자들의 이름 리스트
    public AllEquipParts _allEquipParts = new AllEquipParts();            // 방 이름과 장착한 파츠 정보 리스트

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
            //DontDestroyOnLoad(this.gameObject); // 씬 전환 시에도 파괴되지 않도록 설정 (임시)
        }
    }

    public void InitData()
    {
        analyticsData.Init();
        allPlayerHurtInfo = new AllPlayerHurtInfo(); // 방 이름과 해당 방에서 플레이어에게 피해를 준 공격자들의 이름 리스트 초기화
        _allEquipParts = new AllEquipParts(); // 방 이름과 장착한 파츠 정보 리스트 초기화
    }

    //// 초기화
    //async void Start()
    //{
    //    if (!isActive) return;
    //    await UnityServices.InitializeAsync();
    //    AnalyticsService.Instance.StartDataCollection();
    //    _isInitialized = true;
    //}

    public async void AnalyticsInit()
    {
        return;
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    List<Part> equipedParts = new List<Part>
        //    {
        //        new Part { name = "Part1", count = 1 },
        //        new Part { name = "Part2", count = 2 }
        //    };
        //    EquipParts equipedPartsData = new EquipParts
        //    {
        //        sceneName = "TestScene",
        //        equipedParts = equipedParts
        //    };

        //    SaveEquipParts(equipedPartsData);
        //}
        if (Input.GetKeyDown(KeyCode.U))
        {
            TestSendEquipPartsData();
        }
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
    // 엔딩 타이틀 씬 버튼은 전송을 안함!
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
        Artifact_YSJ artifact = PlayerManager.Instance.PlayerHand.RightHand.GetComponentInChildren<Artifact_YSJ>();
        if (artifact != null)
            artifact.CountEquipParts();
        
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

        // 저장했던 파츠들의 정보들을 JSON으로 변환하고 커스텀 이벤트에 할당
        string artifactjson = JsonUtility.ToJson(_allEquipParts);
        Debug.Log(artifactjson);
        CustomEvent artifactEvent = new CustomEvent("Test_PartsLogEvent")
        {
            { "TestLog", artifactjson }
        };

        // 저장했던 피격 정보들을 JSON으로 변환하고 커스텀 이벤트에 할당
        string hurtjson = JsonUtility.ToJson(allPlayerHurtInfo);
        Debug.Log(hurtjson);
        CustomEvent HurtEvent = new CustomEvent("Test_HurtLogEvent")
        {
            { "TestLog", hurtjson }
        };


        AnalyticsService.Instance.RecordEvent(analyticsEvent);
        AnalyticsService.Instance.RecordEvent(artifactEvent);
        AnalyticsService.Instance.RecordEvent(HurtEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("통계 분석 전송");

        isGameStart = false;
    }

    // 각 방을 나갈 때마다 장착한 파츠 정보 저장
    // Artifact에서 호출해줘야 함
    public void SaveEquipParts(EquipParts equipParts)
    {
        _allEquipParts.equipPartsList.Add(equipParts);
        Debug.Log("아티팩트 장착 파츠 기록 완료");
    }

    public void SavePlayerHurtInfo(PlayerHurtInfo hurtInfo)
    {
        allPlayerHurtInfo.playerHurtInfos.Add(hurtInfo);
        print($"{allPlayerHurtInfo.playerHurtInfos.Count}번째 정보: {hurtInfo.hurtPlayerMonsters}에게 맞은 정보 저장");
    }

    // 데이터 전송
    public void TestSendEquipPartsData()
    {
        string json = JsonUtility.ToJson(_allEquipParts);
        Debug.Log(json);

        // Analytics에 보내는 부분 작성할 예정
    }
}