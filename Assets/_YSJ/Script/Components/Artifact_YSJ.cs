using CKT;
using System;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

public class Artifact_YSJ : MonoBehaviour
{
    // 아티팩트의 기본 능력치
    [Header("기본 능력치")]
    [Header("기본 공격 세팅값")]
    public float Default_NormalAttackPower;
    public float Default_NormalAttackCoolTime;
    public float Default_NormalAttackLife;
    public float Default_NormalBulletSpeed;
    public float Default_NormalAttackDelay;
    public float Default_NormalAttackScale = 1.0f; // 일반 공격 기본 크기

    [Header("스킬 공격 세팅값")]
    public float Default_SkillAttackPower;
    public float Default_SkillAttackCoolTime;
    public float Default_SkillAttackLife;
    public float Default_SkillBulletSpeed;
    public float Default_SkillAttackDelay;
    public float Default_SkillAttackScale = 1.0f; // 스킬 공격 기본 크기

    // 아티팩트가 가지는 스탯포인트
    public int StatPoint;

    // 아티팩트 파츠에 의한 추가 능력치
    [Header("파츠에 의한 추가 능력치")]
    public float Added_NormalAttackPower;
    public float Added_NormalAttackCoolTime;
    public float Added_NormalAttackLife;
    public float Added_NormalBulletSpeed;

    public float Added_SkillAttackPower;
    public float Added_SkillAttackCoolTime;
    public float Added_SkillAttackLife;
    public float Added_SkillBulletSpeed;

    // 쿨타임 타이머
    public float elapsedNormalCoolTime;
    public float elapsedSkillCoolTime;

    // 아티팩트의 현재 능력치
    [Header("현재 능력치")]
    public float Current_NormalAttackPower { get; protected set; }
    public float Current_NormalAttackCoolTime {get; protected set;}
    public float Current_NormalAttackLifeTime {get; protected set;}
    public float Current_NormalBulletSpeed {get; protected set;}

    public float Current_SkillAttackPower {get; protected set;}
    public float Current_SkillAttackCoolTime {get; protected set;}
    public float Current_SkillAttackLifeTime {get; protected set;}
    public float Current_SkillBulletSpeed {get; protected set;}

    // 쏠 방향
    public Vector2 Direction { get; protected set; }
    // 총알이 생성 될 위치
    public Transform firePosition;

    // 마법 프리팹
    public GameObject NormalAttackPrefab;
    public GameObject SkillAttackPrefab;

    // 아티팩트가 가진 슬롯개수, 슬롯에 들어있는 파츠정보
    public int MaxSlotCount;
    public Transform SlotTransform;

    // (노말) 쏘기 전, 쏜 후, 적중 시 액션
    public Action<Artifact_YSJ> BeforeFireNormalAttack; //<발사를 한 아티팩트>
    public Action<Artifact_YSJ, GameObject> AfterFireNormalAttack; // <발사를 한 아티팩트, 생성된 총알 오브젝트>
    public Action<Artifact_YSJ, GameObject, GameObject> HitNormalAttack; // <발사를 한 아티팩트, 생성된 총알 오브젝트, 맞은 오브젝트>

    // (스킬) 쏘기 전, 쏜 후, 적중 시 액션
    public Action<Artifact_YSJ> BeforeFireSkillAttack;
    public Action<Artifact_YSJ, GameObject> AfterFireSkillAttack;
    public Action<Artifact_YSJ, GameObject, GameObject> HitSkillAttack;

    protected void ArtifactInitialize()
    {
        BeforeFireNormalAttack += CountCurrentStatus; // 일반 공격을 하기 전 현재 능력치 계산
    }

    public virtual void NormalAttackTriggered()
    {
        print("일반공격");
        // 일반 공격을 실행하기 전 액션 실행
        BeforeFireNormalAttack?.Invoke(this);

        // 일반 공격 탄 생성
        if (NormalAttackPrefab)
        {
            GameObject SpawnedBullet = Instantiate(NormalAttackPrefab);

            // 일반 공격 생성 한 직후 액션 실행
            AfterFireNormalAttack?.Invoke(this, SpawnedBullet);
        }
    }
    public virtual void NormalAttackCancled()
    {
    }

    public virtual void SkillAttackTriggered()
    {
        print("스킬공격");
        if (SkillAttackPrefab)
        {
            GameObject SpawnedBullet = Instantiate(SkillAttackPrefab);

            AfterFireSkillAttack?.Invoke(this, SpawnedBullet);
        }
    }
    public virtual void SkillAttackCancled()
    {
    }

    protected virtual void CountCurrentStatus(Artifact_YSJ Artifact)
    {
        Current_NormalAttackPower = Default_NormalAttackPower + Added_NormalAttackPower;
        Current_NormalAttackCoolTime = Default_NormalAttackCoolTime + Added_NormalAttackCoolTime;
        Current_NormalAttackLifeTime = Default_NormalAttackLife + Added_NormalAttackLife;
        Current_NormalBulletSpeed = Default_NormalBulletSpeed + Added_NormalBulletSpeed;
    }

    public void AddParts(ImagePartsRoot_YSJ imageParts, int index)
    {
        Destroy(SlotTransform.GetChild(index).gameObject); // 기존 파츠 제거

        GameObject clone = Instantiate(imageParts.gameObject, SlotTransform);
        clone.transform.SetParent(SlotTransform);
        clone.transform.SetSiblingIndex(index); // 마지막에 추가
    }

    public void RemoveParts(int index) 
    {
        Destroy(SlotTransform.GetChild(index).gameObject);

        GameObject Empty = new();
        Empty.transform.SetParent(SlotTransform);
        Empty.transform.SetSiblingIndex(index);
    }
}
