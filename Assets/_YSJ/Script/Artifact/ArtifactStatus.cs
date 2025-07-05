using System;
using UnityEngine;

namespace YSJ
{
    [Serializable]
    public class ArtifactStatus
    {
        #region [기본 능력치 변수들]
        public float Default_AttackPower;
        public float Default_AttackCoolTime;
        public float Default_AttackLife;
        public float Default_BulletSpeed;
        public float Default_AttackStartDelay;
        public float Default_AttackScale = 1.0f;          // 일반 공격 기본 크기
        public int Default_AttackCount = 1;               // 일반 공격 기본 발사 횟수
        public float Default_AttackCountDeltaTime = 0.1f; // 일반 공격 발사 횟수가 여러번이라면 발사 간격 시간
        public int Default_AttackSpreadCount = 1;         // 일반 공격 기본 산탄 횟수
        public float Default_AttackSpreadAngle = 0.0f;    // 일반 공격 퍼짐 각도
        #endregion

        #region [추가 능력치 변수들]
        // 아티팩트 파츠에 의한 추가 능력치
        [Header("파츠에 의한 추가 능력치")]
        public float Added_AttackPower { get; set; }          // 일반 공격 추가 공격력
        public float Added_AttackCoolTime { get; set; }       // 일반 공격 추가 쿨타임
        public float Added_AttackLife { get; set; }           // 일반 공격 추가 지속시간
        public float Added_BulletSpeed { get; set; }          // 일반 공격 추가 속도
        public float Added_AttackStartDelay { get; set; }     // 일반 공격 추가 선딜레이
        public float Added_AttackScale { get; set; }          // 일반 공격 추가 크기
        public int Added_AttackCount { get; set; } = 1;       // 일반 공격 추가 발사 횟수
        public int Added_AttackSpreadCount { get; set; } = 1; // 일반 공격 추가 산탄 횟수
        public float Added_AttackSpreadAngle { get; set; }    // 일반 공격 추가 산탄 퍼짐 각도
        #endregion

        #region [아티팩트 현재 능력치 변수들]
        [Header("현재 능력치")]
        public float Current_AttackPower { get; protected set; }
        public float Current_AttackCoolTime { get; protected set; }
        public float Current_AttackLifeTime { get; protected set; }
        public float Current_BulletSpeed { get; protected set; }
        public float Current_AttackStartDelay { get; protected set; }
        public float Current_AttackScale { get; protected set; }
        public float Current_AttackCount { get; protected set; }
        public float Current_AttackSpreadAngle { get; protected set; }
        #endregion

        [Header("쿨타임 타이머")]
        public float elapsedCoolTime;

        // 코루틴 저장용
        public Coroutine attackCoroutine;

        // 마법 프리팹
        [Header("발사체")]
        public GameObject AttackPrefab;

        // 패시브, 발사 전, 발사 직후 액션
        public Action<Artifact_YSJ> Pessive; //<발사하는 아티팩트>
        public Action<Artifact_YSJ> BeforeFire; // <발사하는 아티팩트>
        public Action<Artifact_YSJ, GameObject> AfterFire; // <발사를 한 아티팩트, 생성된 공격 오브젝트>

        public void ResetAddedStatus()
        {
            Added_AttackPower = 0.0f;
            Added_AttackCoolTime = 0.0f;
            Added_AttackLife = 0.0f;
            Added_BulletSpeed = 0.0f;
            Added_AttackStartDelay = 0.0f;
            Added_AttackScale = 0.0f;
            Added_AttackCount = 1;
            Added_AttackSpreadCount = 1;
            Added_AttackSpreadAngle = 0.0f;

            Pessive = null; // 기존 패시브 액션 초기화
            BeforeFire = null;
            AfterFire = null; // 기존 발사 후 액션 초기화
        }

        // current = default + added 계산
        public void CountCurrentStatus()
        {
            Current_AttackPower = Default_AttackPower + Added_AttackPower;
            Current_AttackCoolTime = Default_AttackCoolTime + Added_AttackCoolTime;
            Current_AttackLifeTime = Default_AttackLife + Added_AttackLife;
            Current_BulletSpeed = Default_BulletSpeed + Added_BulletSpeed;
            Current_AttackStartDelay = Default_AttackStartDelay + Added_AttackStartDelay;
            Current_AttackScale = Default_AttackScale + Added_AttackScale;
            Current_AttackCount = Default_AttackCount + Added_AttackCount;
        }
    }
}