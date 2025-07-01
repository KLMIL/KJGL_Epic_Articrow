using CKT;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using YSJ;

public class Artifact_YSJ : MonoBehaviour
{
    // 아티팩트의 기본 능력치
    #region [기본 능력치 변수들]
    [Header("기본 공격 기본 세팅값")]
    public float Default_NormalAttackPower;
    public float Default_NormalAttackCoolTime;
    public float Default_NormalAttackLife;
    public float Default_NormalBulletSpeed;
    public float Default_NormalAttackStartDelay;
    public float Default_NormalAttackScale = 1.0f; // 일반 공격 기본 크기
    public float Default_NormalAttackCount = 1; // 일반 공격 기본 발사 횟수
    public float Default_NormalAttackSpreadAngle = 0.0f; // 일반 공격 퍼짐 각도

    [Header("스킬 공격 기본 세팅값")]
    public float Default_SkillAttackPower;
    public float Default_SkillAttackCoolTime;
    public float Default_SkillAttackLife;
    public float Default_SkillBulletSpeed;
    public float Default_SkillAttackStartDelay;
    public float Default_SkillAttackScale = 1.0f; // 스킬 공격 기본 크기
    public float Default_SkillAttackCount = 1; // 스킬 공격 기본 발사 횟수
    public float Default_SkillAttackSpreadAngle = 0.0f; // 스킬 공격 기본 발사 각도

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
    public float Added_SkillAttackPower { get; set; }
    public float Added_SkillAttackCoolTime { get; set; }
    public float Added_SkillAttackLife { get; set; }
    public float Added_SkillBulletSpeed { get; set; }
    public float Added_SkillAttackStartDelay { get; set; }
    public float Added_SkillAttackScale { get; set; }
    public float Added_SkillAttackCount { get; set; }
    public float Added_SkillAttackSpreadAngle { get; set; }
    #endregion
    #region [아티팩트 현재 능력치 변수들]
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
    public float Current_SkillAttackStartDelay { get; protected set; }
    public float Current_SkillAttackScale { get; protected set; }
    public float Current_SkillAttackCount { get; protected set; }
    public float Current_SkillAttackSpreadAngle { get; protected set; }
    #endregion
    // 쿨타임 타이머
    protected float elapsedNormalCoolTime;
    protected float elapsedSkillCoolTime;

    // 쏠 방향
    public Vector2 Direction { get; protected set; }
    // 총알이 생성 될 위치
    [Header("발사위치")]
    public Transform firePosition;

    // 마법 프리팹
    [Header("발사체")]
    public GameObject NormalAttackPrefab;
    public GameObject SkillAttackPrefab;

    // (노말) 패시브, 발사 직후 액션
    public Action<Artifact_YSJ> PessiveNormalAttack; //<발사를 한 아티팩트>
    public Action<Artifact_YSJ, GameObject> AfterFireNormalAttack; // <발사를 한 아티팩트, 생성된 공격 오브젝트>

    // (스킬) 쏘기 전, 쏜 후, 적중 시 액션
    public Action<Artifact_YSJ> PessiveSkillAttack;
    public Action<Artifact_YSJ, GameObject> AfterFireSkillAttack;

    // 코루틴 저장용
    public Coroutine normalAttackCoroutine = null;
    public Coroutine skillAttackCoroutine = null;

    #region [일반공격]
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
                ResetNormalAttack();

                // 파츠슬롯 한바퀴 돌면서 등록
                for (int i = 0; i < MaxSlotCount; i++)
                {
                    IImagePartsToNormalAttack_YSJ imageParts = SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
                    if (imageParts != null)
                    {
                        PessiveNormalAttack += imageParts.NormalAttackPessive; // 패시브 액션 등록
                        AfterFireNormalAttack += imageParts.NormalAttackAfterFire; // 발사 후 액션 등록

                    }
                }

                // 패시브 액션 실행
                PessiveNormalAttack?.Invoke(this);

                // 스탯 계산
                NormalAttackCountCurrentStatus();

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
                            SpawnedBullet.GetComponent<MagicRoot_YSJ>().NormalAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                            // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                            for (int partsIndex = 0; partsIndex < MaxSlotCount; partsIndex++)
                            {
                                IImagePartsToNormalAttack_YSJ imageParts = SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
                                if (imageParts != null)
                                {
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().FlyingAction += imageParts.NormalAttackFlying; // 공격 날아가는 중 액션 등록
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().OnHitAction += imageParts.NormalAttackOnHit; // 공격 맞았을 때 액션 등록
                                }
                            }

                            // 일반 공격 생성 한 직후 액션 실행
                            AfterFireNormalAttack?.Invoke(this, SpawnedBullet);
                        }

                        // 쿨타임 적용
                        elapsedNormalCoolTime = Current_NormalAttackCoolTime;
                    }
                    else
                    {
                        print("스킬 공격 프리팹 설정안됌!");
                    }
                }
            }
        }

        normalAttackCoroutine = null; // 저장된 코루틴 초기화
    }

    public virtual void NormalAttackCancled()
    {
    }
    #endregion
    #region [스킬공격]
    public virtual void SkillAttackClicked()
    {
        if (skillAttackCoroutine == null)
        {
            skillAttackCoroutine = StartCoroutine(SkillAttackCoroutine());  
        }
    }

    public virtual IEnumerator SkillAttackCoroutine() 
    {
        // 클릭이 들어와있거나 쿨타임이 남아있으면 계속 실행
        while (Managers.Input.IsPressRightHandAttack || elapsedSkillCoolTime > 0)
        {
            // 쿨타임이 남아있으면 대기
            if (elapsedSkillCoolTime > 0)
            {
                elapsedSkillCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                // 초기화
                ResetSkillAttack();

                // 파츠슬롯 한바퀴 돌면서 등록
                for (int i = 0; i < MaxSlotCount; i++)
                {
                    IImagePartsToSkillAttack_YSJ imageParts = SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
                    if (imageParts != null)
                    {
                        PessiveSkillAttack += imageParts.SkillAttackPessive; // 패시브 액션 등록
                        AfterFireSkillAttack += imageParts.SkillAttackAfterFire; // 발사 후 액션 등록
                    }
                }

                // 패시브 액션 실행
                PessiveSkillAttack?.Invoke(this);

                // 스탯 계산
                SkillAttackCountCurrentStatus();

                // 발사 가능하면 발사시도
                if (true)
                {
                    // 선딜 타이머 시작
                    yield return new WaitForSeconds(Current_SkillAttackStartDelay);

                    // 공격 생성
                    if (SkillAttackPrefab)
                    {
                        // 방향 계산    
                        Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                        float firstAngle = angle - ((Current_SkillAttackSpreadAngle * (Current_SkillAttackCount - 1)) / 2f);

                        // 발사 개수만큼 발사
                        for (int SpawnCount = 0; SpawnCount < Current_SkillAttackCount; SpawnCount++)
                        {
                            GameObject SpawnedBullet = Instantiate(SkillAttackPrefab, firePosition.position, Quaternion.Euler(0, 0, firstAngle + Current_SkillAttackSpreadAngle * SpawnCount)); // 첫 각도부터 시작해서 퍼지는 각도 더하면서 탄 생성
                            SpawnedBullet.transform.localScale = Vector3.one * Current_SkillAttackScale; // 공격 크기 설정
                            SpawnedBullet.GetComponent<MagicRoot_YSJ>().SkillAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                            // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                            for (int partsIndex = 0; partsIndex < MaxSlotCount; partsIndex++)
                            {
                                IImagePartsToSkillAttack_YSJ imageParts = SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
                                if (imageParts != null)
                                {
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().FlyingAction += imageParts.SkillAttackFlying; // 공격 날아가는 중 액션 등록
                                    SpawnedBullet.GetComponent<MagicRoot_YSJ>().OnHitAction += imageParts.SKillAttackOnHit; // 공격 맞았을 때 액션 등록
                                }
                            }

                            // 일반 공격 생성 한 직후 액션 실행
                            AfterFireSkillAttack?.Invoke(this, SpawnedBullet);
                        }

                        // 쿨타임 적용
                        elapsedSkillCoolTime = Current_SkillAttackCoolTime;
                    }
                    else 
                    {
                        print("스킬 공격 프리팹 설정안됌!");
                    }
                }
            }
        }

        skillAttackCoroutine = null; // 저장된 코루틴 초기화
    }

    public virtual void SkillAttackCancled()
    {
    }
    #endregion

    // 아티팩트 슬롯 초기화
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
            spawnedObject.name = "Slot";
            spawnedObject.transform.SetParent(SlotTransform);
        }
    }

    // 스탯 포인트 랜덤으로 나눠갖기
    protected void DevideStatPoint() 
    {
        // 스탯 포인트가 0이 될 때까지
        int[] randomValues = new int[11];
        int currentStatPoint = StatPoint; // 스탯 포인트 복사
        while (currentStatPoint > 0) 
        {
            int randomIndex = UnityEngine.Random.Range(0, randomValues.Length);
            randomValues[randomIndex]++;
            currentStatPoint--;
        }

         Default_NormalAttackPower += randomValues[0];
         Default_NormalAttackCoolTime += randomValues[1] * -0.1f;
         Default_NormalAttackLife += randomValues[2];
         Default_NormalBulletSpeed += randomValues[3];
         Default_NormalAttackStartDelay += randomValues[4] * -0.1f;

         Default_SkillAttackPower += randomValues[5];
         Default_SkillAttackCoolTime += randomValues[6] * -0.1f;
         Default_SkillAttackLife += randomValues[7];
         Default_SkillBulletSpeed += randomValues[8];
         Default_SkillAttackStartDelay += randomValues[9] * -0.1f;

         MaxSlotCount += randomValues[10];
    }

    // Init
    protected void ArtifactInitialize() 
    {
        DevideStatPoint(); // 스탯 포인트 나눠갖기
        SlotRefresh(); // 슬롯 초기화
    }

    // 어택 전 added변수들 초기화
    protected void ResetNormalAttack()
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
    protected void ResetSkillAttack()
    {
        Added_SkillAttackPower = 0.0f;
        Added_SkillAttackCoolTime = 0.0f;
        Added_SkillAttackLife = 0.0f;
        Added_SkillBulletSpeed = 0.0f;
        Added_SkillAttackStartDelay = 0.0f;
        Added_SkillAttackScale = 0.0f;
        Added_SkillAttackCount = 0;
        Added_SkillAttackSpreadAngle = 0.0f;

        PessiveSkillAttack = null; // 기존 패시브 액션 초기화
        AfterFireSkillAttack = null; // 기존 발사 후 액션 초기화
    }

    // current = default + added 계산
    protected virtual void NormalAttackCountCurrentStatus()
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
    protected virtual void SkillAttackCountCurrentStatus()
    {
        Current_SkillAttackPower = Default_SkillAttackPower + Added_SkillAttackPower;
        Current_SkillAttackCoolTime = Default_SkillAttackCoolTime + Added_SkillAttackCoolTime;
        Current_SkillAttackLifeTime = Default_SkillAttackLife + Added_SkillAttackLife;
        Current_SkillBulletSpeed = Default_SkillBulletSpeed + Added_SkillBulletSpeed;
        Current_SkillAttackStartDelay = Default_SkillAttackStartDelay + Added_SkillAttackStartDelay;
        Current_SkillAttackScale = Default_SkillAttackScale + Added_SkillAttackScale;
        Current_SkillAttackCount = Default_SkillAttackCount + Added_SkillAttackCount;
        Current_SkillAttackSpreadAngle = Default_SkillAttackSpreadAngle + Added_SkillAttackSpreadAngle;
    }

    // 파츠 추가, 제거
    public void AddParts(ImagePartsRoot_YSJ imageParts, int index)
    {
        //print("파츠 추가: " + imageParts.name);
        GameObject clone = Instantiate(imageParts.gameObject, SlotTransform);
        clone.transform.SetParent(SlotTransform.GetChild(index));
    }
    public void RemoveParts(int index)
    {
        //print("파츠 제거: " + index);

        Transform targetSlot = SlotTransform.GetChild(index);

        if (targetSlot.childCount == 0) 
        {
            //print("해당 슬롯에 파츠가 없음");
            return;
        }
        else if (targetSlot.childCount > 1) 
        {
            //print("해당 슬롯에 파츠가 여러개 있음. 첫번째 파츠만 제거");
        }

        Destroy(SlotTransform.GetChild(index).GetChild(0).gameObject);
    }
}
