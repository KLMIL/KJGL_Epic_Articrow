using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamAchievement : MonoBehaviour
{
    public enum AchievementType
    {
        Test,
        StartFirstGame,
        TutorialClear,
        SlimeBossClear,
        GolemBossClear
    }

    public static SteamAchievement instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
#if UNITY_EDITOR
        if (SteamManager.Initialized)
        {
            SteamUserStats.ResetAllStats(true);
            SteamUserStats.StoreStats();            // 유저 통계 저장
            Debug.Log("에디터라서 도전과제 초기화하고 시작");
        }
#else
#endif
    }

    void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Achieve(AchievementType.Test);
        }
    }

    public void Achieve(AchievementType achievementType)
    {
        //return;
        string apiName = achievementType.ToString();
        Debug.Log("도전과제 달성 요청");
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)    // 달성하지 않았으면
            {
                SteamUserStats.SetAchievement(apiName); // 도전과제 달성시키기
                SteamUserStats.StoreStats();            // 유저 통계 저장

                Debug.Log($"{apiName} 달성!");
            }
            else
            {
                Debug.Log($"{apiName}는 이미 달성한 도전과제...");
            }
        }
    }
}