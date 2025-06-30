using CKT;
using System;
using System.Collections;
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
    public float Default_NormalAttackStartDelay;
    public float Default_NormalAttackScale = 1.0f; // 일반 공격 기본 크기
    public float Default_NormalAttackCount = 1; // 일반 공격 기본 발사 횟수
    public float Default_NormalAttackSpreadAngle = 0.0f; // 일반 공격 기본 발사 각도 (0이면 직선, 10이면 10도씩 퍼짐)

    [Header("스킬 공격 세팅값")]
    public float Default_SkillAttackPower;
    public float Default_SkillAttackCoolTime;
    public float Default_SkillAttackLife;
    public float Default_SkillBulletSpeed;
    public float Default_SkillAttackStartDelay;
    public float Default_SkillAttackScale = 1.0f; // 스킬 공격 기본 크기

    // 아티팩트가 가지는 스탯포인트
    public int StatPoint;

    // 아티팩트 파츠에 의한 추가 능력치
    [Header("파츠에 의한 추가 능력치")]
    // 일반공격
    public float Added_NormalAttackPower { get; set; } // 일반 공격 추가 공격력
    public float Added_NormalAttackCoolTime { get; set; } // 일반 공격 추가 쿨타임
    public float Added_NormalAttackLife { get; set; } // 일반 공격 추가 지속시간
    public float Added_NormalBulletSpeed { get; set; } // 일반 공격 추가 속도
    public float Added_NormalAttackStartDelay { get; set; } // 일반 공격 추가 선딜레이
    public float Added_NormalAttackScale { get; set; } // 일반 공격 추가 크기
    public float Added_NormalAttackCount { get; set; } // 일반 공격 추가 발사 횟수
    public float Added_NormalAttackSpreadAngle { get; set; } // 일반 공격 추가 발사 각도

    // 스킬공격
    public float Added_SkillAttackPower { protected get; set; }
    public float Added_SkillAttackCoolTime { protected get; set; }
    public float Added_SkillAttackLife { protected get; set; }
    public float Added_SkillBulletSpeed { protected get; set; }

    // 쿨타임 타이머
    public float elapsedNormalCoolTime;
    public float elapsedSkillCoolTime;

    // 아티팩트의 현재 능력치
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

    // 쏠 방향
    public Vector2 Direction { get; protected set; }
    // 총알이 생성 될 위치
    public Transform firePosition;

    // 마법 프리팹
    [Header("발사체")]
    public GameObject NormalAttackPrefab;
    public GameObject SkillAttackPrefab;

    // 아티팩트가 가진 슬롯개수, 슬롯에 들어있는 파츠정보
    [Header("슬롯 수")]
    public int MaxSlotCount;
    public Transform SlotTransform { get; set; }

    // (노말) 패시브, 발사 직후 액션
    public Action<Artifact_YSJ> PessiveNormalAttack; //<발사를 한 아티팩트>
    public Action<Artifact_YSJ, GameObject> AfterFireNormalAttack; // <발사를 한 아티팩트, 생성된 공격 오브젝트>

    // (스킬) 쏘기 전, 쏜 후, 적중 시 액션
    public Action<Artifact_YSJ> PessiveSkillAttack;
    public Action<Artifact_YSJ, GameObject> AfterFireSkillAttack;
    public Action<Artifact_YSJ, GameObject, GameObject> HitSkillAttack;

    // 코루틴 저장용
    protected Coroutine normalAttackCoroutine = null;
    protected Coroutine skillAttackCoroutine = null;

    public virtual void NormalAttackClicked()
    {
        if (normalAttackCoroutine == null) 
        {
            normalAttackCoroutine = StartCoroutine(NormalAttackCoroutine());
        }
    }

    public virtual IEnumerator NormalAttackCoroutine()
    {
        // 클릭이 들어와있거나 쿨타임이 남아있으면 계속 실행
        while (Managers.Input.IsPressLeftHandAttack || elapsedNormalCoolTime > 0) 
        {
            // 쿨타임이 남아있으면 대기
            if (elapsedNormalCoolTime > 0)
            {
                elapsedNormalCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                // 초기화
                ResetArtifact();

                // 파츠슬롯 한바퀴 돌면서 등록
                for (int i = 0; i < MaxSlotCount; i++)
                {
                    IImageParts_YSJ imageParts = SlotTransform.GetChild(i).GetComponent<IImageParts_YSJ>();
                    if (imageParts != null)
                    {
                        PessiveNormalAttack += imageParts.Pessive; // 패시브 액션 등록
                        AfterFireNormalAttack += imageParts.AfterFire; // 발사 후 액션 등록

                    }
                }

                // 패시브 액션 실행
                PessiveNormalAttack?.Invoke(this);

                // 스탯 계산
                CountCurrentStatus();

                // 발사 가능하면 발사시도
                if (true)
                {
                    // 선딜 타이머 시작
                    yield return new WaitForSeconds(Current_NormalAttackStartDelay);

                    // 공격 생성
                    if (NormalAttackPrefab)
                    {
                        // 방향 계산
                        Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                        float firstAngle = angle - ( ( Current_NormalAttackSpreadAngle * (Current_NormalAttackCount - 1) ) / 2f );

                        // 발사 개수만큼 발사
                        for (int SpawnCount = 0; SpawnCount < Current_NormalAttackCount; SpawnCount++) 
                        {
                            GameObject SpawnedBullet = Instantiate(NormalAttackPrefab, firePosition.position, Quaternion.Euler( 0 , 0, firstAngle + Current_NormalAttackSpreadAngle * SpawnCount )); // 첫 각도부터 시작해서 퍼지는 각도 더하면서 탄 생성
                            SpawnedBullet.transform.localScale = Vector3.one * Current_NormalAttackScale; // 공격 크기 설정
                            SpawnedBullet.GetComponent<MagicRoot_YSJ>().BulletInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                            // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                            for (int partsIndex = 0; partsIndex < MaxSlotCount; partsIndex++)
                            {
                                IImageParts_YSJ imageParts = SlotTransform.GetChild(partsIndex).GetComponent<IImageParts_YSJ>();
                                if (imageParts != null)
                                {
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().FlyingAction += imageParts.AttackFlying; // 공격 날아가는 중 액션 등록
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().OnHitAction += imageParts.OnHit; // 공격 맞았을 때 액션 등록
                                }
                            }

                            // 일반 공격 생성 한 직후 액션 실행
                            AfterFireNormalAttack?.Invoke(this, SpawnedBullet);
                        }

                        // 쿨타임 적용
                        elapsedNormalCoolTime = Current_NormalAttackCoolTime;
                    }
                }
            }
        }

        normalAttackCoroutine = null; // 저장된 코루틴 초기화
    }

    public virtual void NormalAttackCancled()
    {
    }

    public virtual void SkillAttackClicked()
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

    protected void SlotRefresh()
    {
        // 싹 슬롯 지워주고
        for (int i = SlotTransform.childCount - 1; i >= 0; i--) 
        {
            Destroy(SlotTransform.GetChild(i).gameObject);
        }

        // 다시 생성
        for (int i = 0; i < MaxSlotCount; i++) 
        {
            GameObject spawnedObject = new();
            spawnedObject.name = "EmptySlot";
            spawnedObject.transform.SetParent(SlotTransform);
        }
    }

    protected void ResetArtifact()
    {
        Added_NormalAttackPower = 0.0f;
        Added_NormalAttackCoolTime = 0.0f;
        Added_NormalAttackLife = 0.0f;
        Added_NormalBulletSpeed = 0.0f;
        Added_NormalAttackStartDelay = 0.0f;
        Added_NormalAttackScale = 0.0f;
        Added_NormalAttackCount = 0;
        Added_NormalAttackSpreadAngle = 0.0f;

        PessiveNormalAttack = null; // 기존 패시브 액션 초기화
        AfterFireNormalAttack = null; // 기존 발사 후 액션 초기화
    }

    protected virtual void CountCurrentStatus()
    {
        Current_NormalAttackPower = Default_NormalAttackPower + Added_NormalAttackPower;
        Current_NormalAttackCoolTime = Default_NormalAttackCoolTime + Added_NormalAttackCoolTime;
        Current_NormalAttackLifeTime = Default_NormalAttackLife + Added_NormalAttackLife;
        Current_NormalBulletSpeed = Default_NormalBulletSpeed + Added_NormalBulletSpeed;
        Current_NormalAttackStartDelay = Default_NormalAttackStartDelay + Added_NormalAttackStartDelay;
        Current_NormalAttackScale = Default_NormalAttackScale + Added_NormalAttackScale;
        Current_NormalAttackCount = Default_NormalAttackCount + Added_NormalAttackCount;
        Current_NormalAttackSpreadAngle = Default_NormalAttackSpreadAngle + Added_NormalAttackSpreadAngle;
    }

    public void AddParts(ImagePartsRoot_YSJ imageParts, int index)
    {
        GameObject emptyParts = SlotTransform.GetChild(index).gameObject;

        GameObject clone = Instantiate(imageParts.gameObject, SlotTransform);
        clone.transform.SetParent(SlotTransform);
        clone.transform.SetSiblingIndex(index); // 인덱스에 파츠 추가

        Destroy(emptyParts); // 기존 빈 슬롯 제거
    }

    public void RemoveParts(int index) 
    {
        GameObject old = SlotTransform.GetChild(index).gameObject;

        GameObject Empty = new();
        Empty.name = "EmptySlott";
        Empty.transform.SetParent(SlotTransform);
        Empty.transform.SetSiblingIndex(index);

        Destroy(old); // 기존 파츠 제거

    }
}
