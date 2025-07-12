using BMC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YSJ;

public class Artifact_YSJ : MonoBehaviour
{
    [Header("능력치")]
    public ArtifactStatus normalStatus = new ArtifactStatus();  // 기본 스테이터스
    public ArtifactStatus skillStatus = new ArtifactStatus();   // 스킬 스테이터스

    [Header("마나 회복량, 소모량")]
    public int ManaIncreaseAmount = 1;
    public int ManaDecreaseAmount = 3;

    [Header("슬롯 수")]
    public int MaxSlotCount;                                    // 아티팩트가 가진 슬롯개수
    public Transform SlotTransform { get; set; }

    [Header("스탯포인트")]
    public int StatPoint;                                       // 아티팩트가 가지는 스탯포인트

    // 플레이어 스테이터스
    public PlayerStatus playerStatus { get; set; }

    [Header("발사 관련")]
    public PlayerHand CurrentHand;                              // 손
    public Vector2 Direction { get; protected set; }            // 쏠 방향
    public Transform firePosition;                              // 총알이 생성 될 위치

    // 조정간
    public bool isCanAttack = true;
    public bool isCanLeftClick = true;
    public bool isCanRightClick = true;

    // 아티팩트 애니메이터
    Animator animator;

    #region 플레이어 강화 관련
    public float Added_MaxHealth { get; set; }
    public float Added_MaxMana { get; set; }
    public float Added_DashCoolTime { get; set; }
    public float Added_MoveSpeed { get; set; }
    #endregion

    #region [일반공격]
    public virtual void NormalAttackClicked()
    {
        if (normalStatus.attackCoroutine == null)
        {
            normalStatus.attackCoroutine = StartCoroutine(NormalAttackCoroutine());
        }
    }

    public virtual IEnumerator NormalAttackCoroutine()
    {
        // 클릭이 들어와있거나 쿨타임이 남아있으면 계속 실행
        while (Managers.Input.IsPressLeftHandAttack || normalStatus.elapsedCoolTime > 0)
        {
            // 쿨타임이 남아있으면 대기 || 공격 불가능한 상황
            if (normalStatus.elapsedCoolTime > 0 || !isCanAttack)
            {
                normalStatus.elapsedCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else if (isCanLeftClick)
            {
                // 초기화
                ResetNormalAttack();
                ReadNormalAttackParts();

                // 패시브 액션 실행
                normalStatus.Pessive?.Invoke(this);

                // 스탯 계산
                normalStatus.CountCurrentStatus();

                // 발사 가능하면 발사시도
                if (true)
                {
                    // 방향 계산
                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                    // 발사 전 액션 실행
                    normalStatus.BeforeFire?.Invoke(this);

                    if (CurrentHand && normalStatus.Current_AttackStartDelay > 0)
                    {
                        CurrentHand.CanHandling = false;
                    }

                    isCanAttack = false;

                    // 선딜 타이머 시작
                    yield return new WaitForSeconds(normalStatus.Current_AttackStartDelay);
                    isCanAttack = true;
                    if (CurrentHand)
                    {
                        CurrentHand.CanHandling = true;
                    }

                    if (normalStatus.AttackPrefab)
                    {
                        int fireCount = 0;
                        // 추가 발사 개수만큼 반복
                        for (int addedSpawnCount = 0; addedSpawnCount < normalStatus.Added_AttackCount; addedSpawnCount++)
                        {
                            // 디폴트 발사 개수만큼 반복
                            for (int SpawnCount = 0; SpawnCount < normalStatus.Default_AttackCount; SpawnCount++)
                            {
                                // Add 산탄만큼 산탄 반복
                                for (int addedSpreadCount = 0; addedSpreadCount < normalStatus.Added_AttackSpreadCount; addedSpreadCount++)
                                {
                                    // 첫번째 발사가 아니라면 방향 다시계산
                                    if (fireCount > 0)
                                    {
                                        Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
                                    }
                                    

                                    float originalAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                                                                                                                 // 추가 산탄의 각도 = (바라보는 각도 - (추가탄의 퍼짐각도 * 추가탄 산탄개수) / 2) + (추가탄의 퍼짐각도 * 추가탄의 산탄횟수 n번째)
                                    float addedSpreadAngle = (originalAngle - (normalStatus.Added_AttackSpreadAngle * (normalStatus.Added_AttackSpreadCount - 1) / 2f)) + (normalStatus.Added_AttackSpreadAngle * addedSpreadCount);

                                    // Default 산탄만큼 산탄 반복
                                    for (int defaultSpreadCount = 0; defaultSpreadCount < normalStatus.Default_AttackSpreadCount; defaultSpreadCount++)
                                    {
                                        // 기본 산탄의 각도 = (추가 산탄의 각도 - (기본탄의 퍼짐 각도 * 기본탄 산탄개수) / 2) + (기본탄의 퍼짐각도 * 기본탄의 산탄횟수 n번째)
                                        float defaultSpreadAngle = (addedSpreadAngle - (normalStatus.Default_AttackSpreadAngle * (normalStatus.Default_AttackSpreadCount - 1) / 2f)) + (normalStatus.Default_AttackSpreadAngle * defaultSpreadCount);

                                        GameObject SpawnedBullet = Instantiate(normalStatus.AttackPrefab, firePosition.position, Quaternion.Euler(0, 0, defaultSpreadAngle)); // 각도에 맞게 탄 생성

                                        SpawnedBullet.transform.localScale = Vector3.one * normalStatus.Current_AttackScale; // 공격 크기 설정
                                        SpawnedBullet.GetComponent<MagicRoot_YSJ>().NormalAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                                        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                                        ReadNormalAttackParts(SpawnedBullet.GetComponent<MagicRoot_YSJ>());

                                        // 맞췄을 때 마나 증가
                                        SpawnedBullet.GetComponent<MagicRoot_YSJ>().OnHitAction += ManaIncrease;

                                        // 일반 공격 생성 한 직후 액션 실행
                                        normalStatus.AfterFire?.Invoke(this, SpawnedBullet);

                                        // 발사 애니메이션 재생
                                        PlayAnimation("Attack");
                                    }
                                }
                                // 마지막 공격이 아니라면 공격 간격만큼 기다리기
                                if (SpawnCount + 1 < normalStatus.Default_AttackCount)
                                {
                                    yield return new WaitForSeconds(normalStatus.Default_AttackCountDeltaTime);
                                }

                                fireCount++;
                            }

                            // 마지막 추가공격이 아니라면 공격 간격만큼 기다리기
                            if (addedSpawnCount + 1 < normalStatus.Added_AttackCount)
                            {
                                yield return new WaitForSeconds(normalStatus.Default_AttackCountDeltaTime);
                            }
                        }

                        // 쿨타임 적용
                        normalStatus.elapsedCoolTime = normalStatus.Current_AttackCoolTime;
                    }
                }
            }
            else
            {
                yield return null;
            }
        }

        normalStatus.attackCoroutine = null; // 저장된 코루틴 초기화
    }

    protected void ReadNormalAttackParts()
    {
        // 파츠슬롯 한바퀴 돌면서 등록
        for (int i = 0; i < MaxSlotCount; i++)
        {
            IImagePartsToNormalAttack_YSJ imageParts = SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
            if (imageParts != null)
            {
                normalStatus.Pessive += imageParts.NormalAttackPessive;         // 패시브 액션 등록
                normalStatus.BeforeFire += imageParts.NormalAttackBeforeFire;   // 발사 전 액션 등록
                normalStatus.AfterFire += imageParts.NormalAttackAfterFire;     // 발사 후 액션 등록
            }
        }
    }

    protected void ReadSkillAttackParts()
    {
        // 파츠슬롯 한바퀴 돌면서 등록
        for (int i = 0; i < MaxSlotCount; i++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
            if (imageParts != null)
            {
                skillStatus.Pessive += imageParts.SkillAttackPessive;       // 패시브 액션 등록
                skillStatus.BeforeFire += imageParts.SkillAttackBeforeFire; // 발사 전 액션 등록
                skillStatus.AfterFire += imageParts.SkillAttackAfterFire;   // 발사 후 액션 등록
            }
        }
    }

    protected void ReadNormalAttackParts(MagicRoot_YSJ magicAttack)
    {
        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
        for (int partsIndex = 0; partsIndex < MaxSlotCount; partsIndex++)
        {
            IImagePartsToNormalAttack_YSJ imageParts = SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
            if (imageParts != null)
            {
                magicAttack.FlyingAction += imageParts.NormalAttackFlying; // 공격 날아가는 중 액션 등록
                magicAttack.OnHitAction += imageParts.NormalAttackOnHit; // 공격 맞았을 때 액션 등록
            }
        }
    }

    protected void ReadSkillAttackParts(MagicRoot_YSJ magicAttack)
    {
        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
        for (int partsIndex = 0; partsIndex < MaxSlotCount; partsIndex++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
            if (imageParts != null)
            {
                magicAttack.FlyingAction += imageParts.SkillAttackFlying; // 공격 날아가는 중 액션 등록
                magicAttack.OnHitAction += imageParts.SKillAttackOnHit; // 공격 맞았을 때 액션 등록
            }
        }
    }

    public virtual void NormalAttackCancled()
    {
    }
    #endregion

    #region [스킬공격]
    public virtual void SkillAttackClicked()
    {
        if (skillStatus.attackCoroutine == null)
        {
            skillStatus.attackCoroutine = StartCoroutine(SkillAttackCoroutine());
        }
    }

    public virtual IEnumerator SkillAttackCoroutine()
    {
        // 클릭이 들어와있거나 쿨타임이 남아있으면 계속 실행
        while (Managers.Input.IsPressRightHandAttack || skillStatus.elapsedCoolTime > 0)
        {
            // 쿨타임이 남아있으면 대기 || 공격 불가능한 상황
            if (skillStatus.elapsedCoolTime > 0 || !isCanAttack)
            {
                skillStatus.elapsedCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else if (isCanRightClick)
            {
                YSJ.Managers.UI.OnStopManaLackEvent.Invoke();
                
                // 초기화
                ResetSkillAttack();

                // 파츠슬롯 한바퀴 돌면서 등록
                ReadSkillAttackParts();

                // 패시브 액션 실행
                skillStatus.Pessive?.Invoke(this);

                // 스탯 계산
                skillStatus.CountCurrentStatus();

                // 발사 가능하면 발사시도(여기서 마나체크를 해야함)
                if (playerStatus.SpendMana(ManaDecreaseAmount))
                {
                    // 방향 계산
                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                    // 발사 전 액션 실행
                    skillStatus.BeforeFire?.Invoke(this);

                    if (CurrentHand)
                    {
                        CurrentHand.CanHandling = false;
                    }
                    isCanAttack = false;

                    // 준비 애니메이션 실행
                    PlayAnimation("Ready");

                    // 선딜 타이머 시작
                    yield return new WaitForSeconds(skillStatus.Current_AttackStartDelay);

                    isCanAttack = true;
                    if (CurrentHand)
                    {
                        CurrentHand.CanHandling = true;
                    }

                    if (skillStatus.AttackPrefab)
                    {
                        int fireCount = 0;

                        // 추가 발사 개수만큼 반복
                        for (int addedSpawnCount = 0; addedSpawnCount < skillStatus.Added_AttackCount; addedSpawnCount++)
                        {
                            // 디폴트 발사 개수만큼 반복
                            for (int SpawnCount = 0; SpawnCount < skillStatus.Default_AttackCount; SpawnCount++)
                            {
                                // 첫번째 발사가 아니라면 방향 다시계산
                                if (fireCount > 0) 
                                {
                                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
                                }

                                // Add 산탄만큼 산탄 반복
                                for (int addedSpreadCount = 0; addedSpreadCount < skillStatus.Added_AttackSpreadCount; addedSpreadCount++)
                                {

                                    float originalAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                                                                                                                 // 추가 산탄의 각도 = (바라보는 각도 - (추가탄의 퍼짐각도 * 추가탄 산탄개수) / 2) + (추가탄의 퍼짐각도 * 추가탄의 산탄횟수 n번째)
                                    float addedSpreadAngle = (originalAngle - (skillStatus.Added_AttackSpreadAngle * (skillStatus.Added_AttackSpreadCount - 1) / 2f)) + (skillStatus.Added_AttackSpreadAngle * addedSpreadCount);

                                    // Default 산탄만큼 산탄 반복
                                    for (int defaultSpreadCount = 0; defaultSpreadCount < skillStatus.Default_AttackSpreadCount; defaultSpreadCount++)
                                    {
                                        // 기본 산탄의 각도 = (추가 산탄의 각도 - (기본탄의 퍼짐 각도 * 기본탄 산탄개수) / 2) + (기본탄의 퍼짐각도 * 기본탄의 산탄횟수 n번째)
                                        float defaultSpreadAngle = (addedSpreadAngle - (skillStatus.Default_AttackSpreadAngle * (skillStatus.Default_AttackSpreadCount - 1) / 2f)) + (skillStatus.Default_AttackSpreadAngle * defaultSpreadCount);

                                        GameObject SpawnedBullet = Instantiate(skillStatus.AttackPrefab, firePosition.position, Quaternion.Euler(0, 0, defaultSpreadAngle)); // 각도에 맞게 탄 생성

                                        SpawnedBullet.transform.localScale = Vector3.one * skillStatus.Current_AttackScale; // 공격 크기 설정
                                        SpawnedBullet.GetComponent<MagicRoot_YSJ>().SkillAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                                        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                                        ReadSkillAttackParts(SpawnedBullet.GetComponent<MagicRoot_YSJ>());

                                        // 일반 공격 생성 한 직후 액션 실행
                                        skillStatus.AfterFire?.Invoke(this, SpawnedBullet);

                                        // 발사 애니메이션 재생
                                        PlayAnimation("Attack");
                                    }
                                }
                                // 마지막 공격이 아니라면 공격 간격만큼 기다리기
                                if (SpawnCount + 1 < skillStatus.Default_AttackCount)
                                {
                                    yield return new WaitForSeconds(skillStatus.Default_AttackCountDeltaTime);
                                }

                                fireCount++;
                            }

                            // 마지막 추가공격이 아니라면 공격 간격만큼 기다리기
                            if (addedSpawnCount + 1 < skillStatus.Added_AttackCount)
                            {
                                yield return new WaitForSeconds(skillStatus.Default_AttackCountDeltaTime);
                            }
                        }
                        // 쿨타임 적용
                        skillStatus.elapsedCoolTime = skillStatus.Current_AttackCoolTime;
                    }
                }
                else 
                {
                    Managers.UI.OnManaLackEvent.Invoke();

                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        skillStatus.attackCoroutine = null; // 저장된 코루틴 초기화
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
    #region [스탯 분배 로직]
    // 스탯 포인트 랜덤으로 나눠갖기

    // 변동시킬 값을 저장 할 구조체
    public class Values
    {
        public Values(float value, float multiply, int index)
        {
            _value = value;
            _variationValue = multiply;
            _index = index;
        }

        public float _value;
        public float _variationValue;
        public int _index;
    }

    protected void DevideStatPoint()
    {
        // first는 값, second는 배율
        List<Values> defaultValues = new();
        // 노말스탯 변경값
        defaultValues.Add(new Values(normalStatus.Default_AttackPower, -1, 0));
        //defaultValues.Add(new Values(normalStatus.Default_AttackCoolTime, 0.1f, 1));
        defaultValues.Add(new Values(normalStatus.Default_BulletLife, -0.05f, 1));
        defaultValues.Add(new Values(normalStatus.Default_AttackStartDelay, 0.1f, 2));

        // 스킬스탯 변경값
        defaultValues.Add(new Values(skillStatus.Default_AttackPower, -1, 3));
        //defaultValues.Add(new Values(skillStatus.Default_AttackCoolTime, 0.1f, 5));
        defaultValues.Add(new Values(skillStatus.Default_BulletLife, -0.05f, 4));
        defaultValues.Add(new Values(skillStatus.Default_AttackStartDelay, 0.1f, 5));

        // 슬롯 수
        defaultValues.Add(new Values(MaxSlotCount, -1, 6));
        defaultValues.Add(new Values(MaxSlotCount, -1, 7));

        // 스탯 포인트 복사
        int currentStatPoint = StatPoint;

        // 리스트 복사
        List<Values> copyValues = defaultValues.Select(v => new Values(v._value, v._variationValue, v._index)).ToList(); // 얕은복사하면 문제생김;

        // 스탯 포인트가 0이 될 때까지
        int InfiniteLoopPrevention = 0;
        while (currentStatPoint > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, copyValues.Count);

            // 현재값 - 변동값 > 0 이면
            if (copyValues[randomIndex]._value - copyValues[randomIndex]._variationValue > 0)
            {
                // 빼주고 스탯포인트 1 차감
                copyValues[randomIndex]._value -= copyValues[randomIndex]._variationValue;
                currentStatPoint--;
            }
            else
            {
                // 0보다 작아진다면 default저장값에 변동값 옮기고 copy에서 삭제(스탯 가져갈 애들 리스트에서 빼주는 거)
                defaultValues[copyValues[randomIndex]._index] = copyValues[randomIndex];
                copyValues.Remove(copyValues[randomIndex]);
            }

            // 무한루프방지
            InfiniteLoopPrevention++;
            if (InfiniteLoopPrevention > 9999)
            {
                print("무한루프!");
                break;
            }

        }

        // 스탯 다돌았는데 카피리스트가 남았다면 디폴트값으로 옮겨주기
        foreach (Values value in copyValues)
        {
            defaultValues[value._index] = value;
        }

        // 변동값 적용 시켜주기
        normalStatus.Default_AttackPower = defaultValues[0]._value;
        normalStatus.Default_BulletLife = defaultValues[1]._value;
        normalStatus.Default_AttackStartDelay = defaultValues[2]._value;

        skillStatus.Default_AttackPower = defaultValues[3]._value;
        skillStatus.Default_BulletLife = defaultValues[4]._value;
        skillStatus.Default_AttackStartDelay = defaultValues[5]._value;

        MaxSlotCount = (int)defaultValues[6]._value + (int)defaultValues[7]._value;
        MaxSlotCount = Mathf.Clamp(MaxSlotCount, 0, 15);
    }
    #endregion

    // Init
    protected void ArtifactInitialize()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
        animator = GetComponent<Animator>();
        DevideStatPoint(); // 스탯 포인트 나눠갖기
        SlotRefresh(); // 슬롯 초기화
    }

    // 공격 전 added변수들 초기화
    protected virtual void ResetNormalAttack()
    {
        normalStatus.ResetAddedStatus();
    }

    protected virtual void ResetSkillAttack()
    {
        skillStatus.ResetAddedStatus();
    }

    // 맞췄을 때 액션에 넣을거임
    protected virtual void ManaIncrease(Artifact_YSJ me, GameObject bullet, GameObject hitObject)
    {
        if (playerStatus)
        {
            playerStatus.RegenerateMana(ManaIncreaseAmount);
        }
        else 
        {
            Debug.LogError("playerStatus 못찾음!");
        }
    }

    //애니메이션 실행
    public void PlayAnimation(string name) 
    {
        if (animator)
        {
            animator.Play(name, 0, 0);
        }
        else
        {
            print("애니메이터 없음!");
        }
    }

    #region 파츠 추가 및 제거
    // 파츠 추가
    public void AddParts(ImagePartsRoot_YSJ imageParts, int index)
    {
        //print("파츠 추가: " + imageParts.name);
        GameObject clone = Instantiate(imageParts.gameObject, SlotTransform);
        clone.transform.SetParent(SlotTransform.GetChild(index));

        UpdateEnhance();
    }

    // 파츠 제거
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

        SlotTransform.GetChild(index).GetChild(0).GetComponent<ImagePartsRoot_YSJ>().WillDestroy = true;
        Destroy(SlotTransform.GetChild(index).GetChild(0).gameObject);

        UpdateEnhance();
    }
    #endregion

    #region 플레이어 강화 관련
    public void UpdateEnhance()
    {
        ResetPlayerEnhance();
        for (int i = 0; i < MaxSlotCount; i++)
        {
            ImagePartsRoot_YSJ imagepartsRoot = SlotTransform.GetChild(i).GetComponentInChildren<ImagePartsRoot_YSJ>();
            if (imagepartsRoot)
            {
                IImagePartsToEnhance_YSJ imageParts = imagepartsRoot.GetComponentInChildren<IImagePartsToEnhance_YSJ>();
                if (imageParts != null && !imagepartsRoot.WillDestroy)
                {
                    imageParts.Equip(this);
                }
            }
        }

        ApplyEnhance();
    }

    void ApplyEnhance()
    {
        playerStatus = PlayerManager.Instance.PlayerStatus;
        playerStatus.OffsetMaxHealth = Added_MaxHealth;
        playerStatus.OffsetMaxMana = Added_MaxMana;
        playerStatus.OffsetDashCoolTime = Added_DashCoolTime;
        playerStatus.OffsetMoveSpeed = Added_MoveSpeed;
    }

    public void ResetPlayerEnhance()
    {
        Added_MaxHealth = 0.0f;
        Added_MaxMana = 0.0f;
        Added_DashCoolTime = 0.0f;
        Added_MoveSpeed = 0.0f;
    }
    #endregion
}