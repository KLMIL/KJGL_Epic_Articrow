using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;

    int testLevel = 0;

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

    // 초기화
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            NextLevel(testLevel); // 예시로 1번 레벨로 이동
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            RestartGame();
        }
    }

    public void NextLevel(int currentLevel)
    {
        if(!_isInitialized)
        {
            return;
        }

        // 이벤트 생성
        CustomEvent myEvent = new CustomEvent("next_level")
        {
            // 해당 이벤트의 파라미터 설정
            { "level_index", currentLevel }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("next_level");
        testLevel++;
    }

    public void RestartGame()
    {
        AnalyticsService.Instance.RecordEvent("restart_game");
        AnalyticsService.Instance.Flush();
        Debug.Log("restart_game");
    }
}