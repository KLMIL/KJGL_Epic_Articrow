using UnityEngine;

[System.Serializable]
public class ArtifactStatus
{
    #region [기본 능력치 변수들]
    [Header("기본 공격 기본 세팅값")]
    public float Default_NormalAttackPower;
    public float Default_NormalAttackCoolTime;
    public float Default_NormalAttackLife;
    public float Default_NormalBulletSpeed;
    public float Default_NormalAttackStartDelay;
    public float Default_NormalAttackScale = 1.0f;          // 일반 공격 기본 크기
    public int Default_NormalAttackCount = 1;               // 일반 공격 기본 발사 횟수
    public float Default_NormalAttackCountDeltaTime = 0.1f; // 일반 공격 발사 횟수가 여러번이라면 발사 간격 시간
    public int Default_NormalAttackSpreadCount = 1;         // 일반 공격 기본 산탄 횟수
    public float Default_NormalAttackSpreadAngle = 0.0f;    // 일반 공격 퍼짐 각도

    [Header("스킬 공격 기본 세팅값")]
    public float Default_SkillAttackPower;
    public float Default_SkillAttackCoolTime;
    public float Default_SkillAttackLife;
    public float Default_SkillBulletSpeed;
    public float Default_SkillAttackStartDelay;
    public float Default_SkillAttackScale = 1.0f;           // 스킬 공격 기본 크기
    public int Default_SkillAttackCount = 1;                // 스킬 공격 기본 발사 횟수
    public float Default_SkillAttackCountDeltaTime = 0.1f;  // 스킬 발사 횟수가 여러번이라면 발사 간격 시간
    public int Default_SkillAttackSpreadCount = 1;          // 스킬 기본 산탄 횟수
    public float Default_SkillAttackSpreadAngle = 0.0f;     // 스킬 공격 기본 퍼짐 각도

    // 아티팩트가 가진 슬롯개수, 슬롯에 들어있는 파츠정보
    [Header("슬롯 수")]
    public int MaxSlotCount;
    public Transform SlotTransform { get; set; }

    // 아티팩트가 가지는 스탯포인트
    [Header("스탯포인트")]
    public int StatPoint;
    #endregion

    #region [추가 능력치 변수들]
    // 아티팩트 파츠에 의한 추가 능력치
    [Header("파츠에 의한 추가 능력치")]
    // 플레이어 강화
    public float Added_MaxHealth { get; set; }
    public float Added_MaxMana { get; set; }
    public float Added_DeahCoolTime { get; set; }
    public float Added_MoveSpeed { get; set; }

    // 일반공격
    public float Added_NormalAttackPower { get; set; }          // 일반 공격 추가 공격력
    public float Added_NormalAttackCoolTime { get; set; }       // 일반 공격 추가 쿨타임
    public float Added_NormalAttackLife { get; set; }           // 일반 공격 추가 지속시간
    public float Added_NormalBulletSpeed { get; set; }          // 일반 공격 추가 속도
    public float Added_NormalAttackStartDelay { get; set; }     // 일반 공격 추가 선딜레이
    public float Added_NormalAttackScale { get; set; }          // 일반 공격 추가 크기
    public int Added_NormalAttackCount { get; set; } = 1;       // 일반 공격 추가 발사 횟수
    public int Added_NormalAttackSpreadCount { get; set; } = 1; // 일반 공격 추가 산탄 횟수
    public float Added_NormalAttackSpreadAngle { get; set; }    // 일반 공격 추가 산탄 퍼짐 각도

    // 스킬공격
    public float Added_SkillAttackPower { get; set; }
    public float Added_SkillAttackCoolTime { get; set; }
    public float Added_SkillAttackLife { get; set; }
    public float Added_SkillBulletSpeed { get; set; }
    public float Added_SkillAttackStartDelay { get; set; }
    public float Added_SkillAttackScale { get; set; }
    public int Added_SkillAttackCount { get; set; } = 1;
    public int Added_SkillAttackSpreadCount { get; set; } = 1;
    public float Added_SkillAttackSpreadAngle { get; set; }
    #endregion

    #region [아티팩트 현재 능력치 변수들]
    [Header("현재 능력치")]
    public float Current_NormalAttackPower { get; protected set; }
    public float Current_NormalAttackCoolTime { get; protected set; }
    public float Current_NormalAttackLifeTime { get; protected set; }
    public float Current_NormalBulletSpeed { get; protected set; }
    public float Current_NormalAttackStartDelay { get; protected set; }
    public float Current_NormalAttackScale { get; protected set; }
    public float Current_NormalAttackCount { get; protected set; }
    public float Current_NormalAttackSpreadAngle { get; protected set; }

    public float Current_SkillAttackPower { get; protected set; }
    public float Current_SkillAttackCoolTime { get; protected set; }
    public float Current_SkillAttackLifeTime { get; protected set; }
    public float Current_SkillBulletSpeed { get; protected set; }
    public float Current_SkillAttackStartDelay { get; protected set; }
    public float Current_SkillAttackScale { get; protected set; }
    public float Current_SkillAttackCount { get; protected set; }
    public float Current_SkillAttackSpreadAngle { get; protected set; }
    #endregion

    public void ResetNormalAddedStatus()
    {
        Added_NormalAttackPower = 0.0f;
        Added_NormalAttackCoolTime = 0.0f;
        Added_NormalAttackLife = 0.0f;
        Added_NormalBulletSpeed = 0.0f;
        Added_NormalAttackStartDelay = 0.0f;
        Added_NormalAttackScale = 0.0f;

        Added_NormalAttackCount = 1;

        Added_NormalAttackSpreadCount = 1;
        Added_NormalAttackSpreadAngle = 0.0f;
    }

    public void ResetSkillAddedStatus()
    {
        Added_SkillAttackPower = 0.0f;
        Added_SkillAttackCoolTime = 0.0f;
        Added_SkillAttackLife = 0.0f;
        Added_SkillBulletSpeed = 0.0f;
        Added_SkillAttackStartDelay = 0.0f;
        Added_SkillAttackScale = 0.0f;

        Added_SkillAttackCount = 1;

        Added_SkillAttackSpreadCount = 1;
        Added_SkillAttackSpreadAngle = 0.0f;
    }

    public void ResetPlayerEnhance()
    {
        Added_MaxHealth = 0.0f;
        Added_MaxMana = 0.0f;
        Added_DeahCoolTime = 0.0f;
        Added_MoveSpeed = 0.0f;
    }

    // current = default + added 계산
    public void NormalAttackCountCurrentStatus()
    {
        Current_NormalAttackPower = Default_NormalAttackPower + Added_NormalAttackPower;
        Current_NormalAttackCoolTime = Default_NormalAttackCoolTime + Added_NormalAttackCoolTime;
        Current_NormalAttackLifeTime = Default_NormalAttackLife + Added_NormalAttackLife;
        Current_NormalBulletSpeed = Default_NormalBulletSpeed + Added_NormalBulletSpeed;
        Current_NormalAttackStartDelay = Default_NormalAttackStartDelay + Added_NormalAttackStartDelay;
        Current_NormalAttackScale = Default_NormalAttackScale + Added_NormalAttackScale;
        Current_NormalAttackCount = Default_NormalAttackCount + Added_NormalAttackCount;
    }
    public void SkillAttackCountCurrentStatus()
    {
        Current_SkillAttackPower = Default_SkillAttackPower + Added_SkillAttackPower;
        Current_SkillAttackCoolTime = Default_SkillAttackCoolTime + Added_SkillAttackCoolTime;
        Current_SkillAttackLifeTime = Default_SkillAttackLife + Added_SkillAttackLife;
        Current_SkillBulletSpeed = Default_SkillBulletSpeed + Added_SkillBulletSpeed;
        Current_SkillAttackStartDelay = Default_SkillAttackStartDelay + Added_SkillAttackStartDelay;
        Current_SkillAttackScale = Default_SkillAttackScale + Added_SkillAttackScale;
        Current_SkillAttackCount = Default_SkillAttackCount + Added_SkillAttackCount;
    }
}