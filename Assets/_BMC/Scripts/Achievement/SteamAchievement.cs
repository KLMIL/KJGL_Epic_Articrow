using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamAchievement : MonoBehaviour
{
    public static SteamAchievement instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            //Debug.LogError("아아 테스트 준비 완료");
            Achieve("NEW_ACHIEVEMENT_1_0");
        }
    }

    public void Achieve(string apiName)
    {
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