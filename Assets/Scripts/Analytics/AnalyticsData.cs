
using System;
using System.Collections.Generic;

[System.Serializable]
public class AnalyticsData
{
    public int countArtifactSwap;      // 아티팩트 교체 횟수
    public int playerHurtCount;        // 적에게 공격 받은 횟수
    public int normalAttackCount;      // 일반 공격 횟수
    public int skillAttackCount;       // 스킬 공격 횟수
    public bool isPlayerDead;          // 플레이어가 죽었는지 여부
    public int currentEquipPartsCount; // 현재 장착된 파츠 수
    public int playTime;               // 플레이 타임
    public int progressStage;          // 진행 중인 스테이지

    public void Init()
    {
        countArtifactSwap = 0;
        playerHurtCount = 0;
        normalAttackCount = 0;
        skillAttackCount = 0;
        isPlayerDead = false;
        currentEquipPartsCount = 0;
        playTime = 0;
        progressStage = 0;
    }
}