using BMC;
using System;
using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_YSJ : MonoBehaviour
{
    // 플레이어 스테이터스
    public PlayerStatus playerStatus { get; set; }

    public ArtifactStatus artifactStatus = new ArtifactStatus();    // 아티팩트 스테이터스

    // 쿨타임 타이머
    protected float elapsedNormalCoolTime;
    protected float elapsedSkillCoolTime;

    // 손 찾기
    public PlayerHand currentHand;

    // 쏠 방향
    public Vector2 Direction { get; protected set; }
    // 총알이 생성 될 위치
    [Header("발사위치")]
    public Transform firePosition;

    // 마법 프리팹
    [Header("발사체")]
    public GameObject NormalAttackPrefab;
    public GameObject SkillAttackPrefab;

    // (노말) 패시브, 발사 전, 발사 직후 액션
    public Action<Artifact_YSJ> PessiveNormalAttack; //<발사하는 아티팩트>
    public Action<Artifact_YSJ> BeforeFireNormalAttack; // <발사하는 아티팩트>
    public Action<Artifact_YSJ, GameObject> AfterFireNormalAttack; // <발사를 한 아티팩트, 생성된 공격 오브젝트>

    // (스킬) 패시브, 발사 전, 발사 직후 액션
    public Action<Artifact_YSJ> PessiveSkillAttack;
    public Action<Artifact_YSJ> BeforeFireSkillAttack;
    public Action<Artifact_YSJ, GameObject> AfterFireSkillAttack;

    // 코루틴 저장용
    public Coroutine normalAttackCoroutine = null;
    public Coroutine skillAttackCoroutine = null;

    // 조정간
    public bool isCanAttack = true;
    public bool isCanLeftClick = true;
    public bool isCanRightClick = true;

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
            // 쿨타임이 남아있으면 대기 || 공격 불가능한 상황
            if (elapsedNormalCoolTime > 0 || !isCanAttack)
            {
                elapsedNormalCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else if (isCanLeftClick)
            {
                // 초기화
                ResetNormalAttack();
                ReadNormalAttackParts();

                // 패시브 액션 실행
                PessiveNormalAttack?.Invoke(this);

                // 스탯 계산
                //NormalAttackCountCurrentStatus();
                artifactStatus.NormalAttackCountCurrentStatus();

                // 발사 가능하면 발사시도
                if (true)
                {
                    // 방향 계산
                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                    // 발사 전 액션 실행
                    BeforeFireNormalAttack?.Invoke(this);

                    if (currentHand && artifactStatus.Current_NormalAttackStartDelay > 0)
                    {
                        currentHand.CanHandling = false;
                    }
                    isCanAttack = false;
                    // 선딜 타이머 시작
                    yield return new WaitForSeconds(artifactStatus.Current_NormalAttackStartDelay);
                    isCanAttack = true;
                    if (currentHand)
                    {
                        currentHand.CanHandling = true;
                    }

                    // 공격 생성
                    if (NormalAttackPrefab)
                    {
                        // 추가 발사 개수만큼 반복
                        for (int addedSpawnCount = 0; addedSpawnCount < artifactStatus.Added_NormalAttackCount; addedSpawnCount++)
                        {
                            // 디폴트 발사 개수만큼 반복
                            for (int SpawnCount = 0; SpawnCount < artifactStatus.Default_NormalAttackCount; SpawnCount++)
                            {
                                // Add 산탄만큼 산탄 반복
                                for (int addedSpreadCount = 0; addedSpreadCount < artifactStatus.Added_NormalAttackSpreadCount; addedSpreadCount++)
                                {
                                    // 발사하기 전에 방향 다시계산
                                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
                                    float originalAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                                                                                                                 // 추가 산탄의 각도 = (바라보는 각도 - (추가탄의 퍼짐각도 * 추가탄 산탄개수) / 2) + (추가탄의 퍼짐각도 * 추가탄의 산탄횟수 n번째)
                                    float addedSpreadAngle = (originalAngle - (artifactStatus.Added_NormalAttackSpreadAngle * (artifactStatus.Added_NormalAttackSpreadCount - 1) / 2f)) + (artifactStatus.Added_NormalAttackSpreadAngle * addedSpreadCount);

                                    // Default 산탄만큼 산탄 반복
                                    for (int defaultSpreadCount = 0; defaultSpreadCount < artifactStatus.Default_NormalAttackSpreadCount; defaultSpreadCount++)
                                    {
                                        // 기본 산탄의 각도 = (추가 산탄의 각도 - (기본탄의 퍼짐 각도 * 기본탄 산탄개수) / 2) + (기본탄의 퍼짐각도 * 기본탄의 산탄횟수 n번째)
                                        float defaultSpreadAngle = (addedSpreadAngle - (artifactStatus.Default_NormalAttackSpreadAngle * (artifactStatus.Default_NormalAttackSpreadCount - 1) / 2f)) + (artifactStatus.Default_NormalAttackSpreadAngle * defaultSpreadCount);

                                        GameObject SpawnedBullet = Instantiate(NormalAttackPrefab, firePosition.position, Quaternion.Euler(0, 0, defaultSpreadAngle)); // 각도에 맞게 탄 생성

                                        SpawnedBullet.transform.localScale = Vector3.one * artifactStatus.Current_NormalAttackScale; // 공격 크기 설정
                                        SpawnedBullet.GetComponent<MagicRoot_YSJ>().NormalAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                                        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                                        ReadNormalAttackParts(SpawnedBullet.GetComponent<MagicRoot_YSJ>());

                                        // 일반 공격 생성 한 직후 액션 실행
                                        AfterFireNormalAttack?.Invoke(this, SpawnedBullet);
                                    }
                                }
                                // 마지막 공격이 아니라면 공격 간격만큼 기다리기
                                if (SpawnCount + 1 < artifactStatus.Default_NormalAttackCount)
                                {
                                    yield return new WaitForSeconds(artifactStatus.Default_NormalAttackCountDeltaTime);
                                }
                            }

                            // 마지막 추가공격이 아니라면 공격 간격만큼 기다리기
                            if (addedSpawnCount + 1 < artifactStatus.Added_NormalAttackCount)
                            {
                                yield return new WaitForSeconds(artifactStatus.Default_NormalAttackCountDeltaTime);
                            }
                        }

                        // 쿨타임 적용
                        elapsedNormalCoolTime = artifactStatus.Current_NormalAttackCoolTime;
                    }
                    else
                    {
                        print("스킬 공격 프리팹 설정안됌!");
                    }
                }
            }
            else
            {
                yield return null;
            }
        }

        normalAttackCoroutine = null; // 저장된 코루틴 초기화
    }

    protected void ReadNormalAttackParts()
    {
        // 파츠슬롯 한바퀴 돌면서 등록
        for (int i = 0; i < artifactStatus.MaxSlotCount; i++)
        {
            IImagePartsToNormalAttack_YSJ imageParts = artifactStatus.SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
            if (imageParts != null)
            {
                PessiveNormalAttack += imageParts.NormalAttackPessive; // 패시브 액션 등록
                AfterFireNormalAttack += imageParts.NormalAttackAfterFire; // 발사 후 액션 등록

            }
        }
    }
    protected void ReadSkillAttackParts()
    {
        // 파츠슬롯 한바퀴 돌면서 등록
        for (int i = 0; i < artifactStatus.MaxSlotCount; i++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = artifactStatus.SlotTransform.GetChild(i).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
            if (imageParts != null)
            {
                PessiveSkillAttack += imageParts.SkillAttackPessive; // 패시브 액션 등록
                AfterFireSkillAttack += imageParts.SkillAttackAfterFire; // 발사 후 액션 등록
            }
        }
    }
    protected void ReadNormalAttackParts(MagicRoot_YSJ magicAttack)
    {
        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
        for (int partsIndex = 0; partsIndex < artifactStatus.MaxSlotCount; partsIndex++)
        {
            IImagePartsToNormalAttack_YSJ imageParts = artifactStatus.SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToNormalAttack_YSJ>();
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
        for (int partsIndex = 0; partsIndex < artifactStatus.MaxSlotCount; partsIndex++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = artifactStatus.SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
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
            // 쿨타임이 남아있으면 대기 || 공격 불가능한 상황
            if (elapsedSkillCoolTime > 0 || !isCanAttack)
            {
                elapsedSkillCoolTime -= Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            else if (isCanRightClick)
            {
                // 초기화
                ResetSkillAttack();

                // 파츠슬롯 한바퀴 돌면서 등록
                ReadSkillAttackParts();

                // 패시브 액션 실행
                PessiveSkillAttack?.Invoke(this);

                // 스탯 계산
                artifactStatus.SkillAttackCountCurrentStatus();
                //SkillAttackCountCurrentStatus();

                // 발사 가능하면 발사시도(여기서 마나체크를 해야함)
                if (true)
                {
                    // 방향 계산
                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;

                    // 발사 전 액션 실행
                    BeforeFireSkillAttack?.Invoke(this);

                    if (currentHand)
                    {
                        currentHand.CanHandling = false;
                    }
                    isCanAttack = false;
                    // 선딜 타이머 시작
                    if (artifactStatus.Current_SkillAttackStartDelay > 0)
                    {
                        yield return new WaitForSeconds(artifactStatus.Current_SkillAttackStartDelay);
                    }
                    isCanAttack = true;
                    if (currentHand)
                    {
                        currentHand.CanHandling = true;
                    }

                    // 공격 생성
                    if (SkillAttackPrefab)
                    {
                        // 추가 발사 개수만큼 반복
                        for (int addedSpawnCount = 0; addedSpawnCount < artifactStatus.Added_SkillAttackCount; addedSpawnCount++)
                        {
                            // 디폴트 발사 개수만큼 반복
                            for (int SpawnCount = 0; SpawnCount < artifactStatus.Default_SkillAttackCount; SpawnCount++)
                            {
                                // 발사하기 전에 방향 다시계산
                                Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
                                // Add 산탄만큼 산탄 반복
                                for (int addedSpreadCount = 0; addedSpreadCount < artifactStatus.Added_SkillAttackSpreadCount; addedSpreadCount++)
                                {

                                    float originalAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
                                                                                                                 // 추가 산탄의 각도 = (바라보는 각도 - (추가탄의 퍼짐각도 * 추가탄 산탄개수) / 2) + (추가탄의 퍼짐각도 * 추가탄의 산탄횟수 n번째)
                                    float addedSpreadAngle = (originalAngle - (artifactStatus.Added_SkillAttackSpreadAngle * (artifactStatus.Added_SkillAttackSpreadCount - 1) / 2f)) + (artifactStatus.Added_SkillAttackSpreadAngle * addedSpreadCount);

                                    // Default 산탄만큼 산탄 반복
                                    for (int defaultSpreadCount = 0; defaultSpreadCount < artifactStatus.Default_SkillAttackSpreadCount; defaultSpreadCount++)
                                    {
                                        // 기본 산탄의 각도 = (추가 산탄의 각도 - (기본탄의 퍼짐 각도 * 기본탄 산탄개수) / 2) + (기본탄의 퍼짐각도 * 기본탄의 산탄횟수 n번째)
                                        float defaultSpreadAngle = (addedSpreadAngle - (artifactStatus.Default_SkillAttackSpreadAngle * (artifactStatus.Default_SkillAttackSpreadCount - 1) / 2f)) + (artifactStatus.Default_SkillAttackSpreadAngle * defaultSpreadCount);

                                        GameObject SpawnedBullet = Instantiate(SkillAttackPrefab, firePosition.position, Quaternion.Euler(0, 0, defaultSpreadAngle)); // 각도에 맞게 탄 생성

                                        SpawnedBullet.transform.localScale = Vector3.one * artifactStatus.Current_SkillAttackScale; // 공격 크기 설정
                                        SpawnedBullet.GetComponent<MagicRoot_YSJ>().SkillAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                                        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                                        ReadSkillAttackParts(SpawnedBullet.GetComponent<MagicRoot_YSJ>());

                                        // 일반 공격 생성 한 직후 액션 실행
                                        AfterFireSkillAttack?.Invoke(this, SpawnedBullet);
                                    }
                                }
                                // 마지막 공격이 아니라면 공격 간격만큼 기다리기
                                if (SpawnCount + 1 < artifactStatus.Default_SkillAttackCount)
                                {
                                    yield return new WaitForSeconds(artifactStatus.Default_SkillAttackCountDeltaTime);
                                }
                            }

                            // 마지막 추가공격이 아니라면 공격 간격만큼 기다리기
                            if (addedSpawnCount + 1 < artifactStatus.Added_SkillAttackCount)
                            {
                                yield return new WaitForSeconds(artifactStatus.Default_SkillAttackCountDeltaTime);
                            }
                        }
                        // 쿨타임 적용
                        elapsedSkillCoolTime = artifactStatus.Current_SkillAttackCoolTime;
                    }
                    else
                    {
                        print("스킬 공격 프리팹 설정안됌!");
                    }
                }
            }
            else
            {
                yield return null;
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
        for (int i = artifactStatus.SlotTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(artifactStatus.SlotTransform.GetChild(i).gameObject);
        }

        // 다시 생성
        for (int i = 0; i < artifactStatus.MaxSlotCount; i++)
        {
            GameObject spawnedObject = new();
            spawnedObject.name = "Slot";
            spawnedObject.transform.SetParent(artifactStatus.SlotTransform);
        }
    }

    // 스탯 포인트 랜덤으로 나눠갖기
    protected void DevideStatPoint()
    {
        // 스탯 포인트가 0이 될 때까지
        int[] randomValues = new int[11];
        int currentStatPoint = artifactStatus.StatPoint; // 스탯 포인트 복사
        while (currentStatPoint > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, randomValues.Length);
            randomValues[randomIndex]++;
            currentStatPoint--;
        }

        artifactStatus.Default_NormalAttackPower += randomValues[0];

        float newDefaultNormalAttackCoolTime = artifactStatus.Default_NormalAttackCoolTime - randomValues[1] * 0.1f;
        artifactStatus.Default_NormalAttackCoolTime = Mathf.Clamp(newDefaultNormalAttackCoolTime, 0, artifactStatus.Default_NormalAttackCoolTime);
        //Default_NormalAttackCoolTime += randomValues[1] * -0.1f;
        artifactStatus.Default_NormalAttackLife += randomValues[2];
        artifactStatus.Default_NormalBulletSpeed += randomValues[3];

        float newDefaultNormalAttackStartDelay = artifactStatus.Default_NormalAttackStartDelay - randomValues[4] * 0.1f;
        artifactStatus.Default_NormalAttackStartDelay = Mathf.Clamp(newDefaultNormalAttackStartDelay, 0, artifactStatus.Default_NormalAttackStartDelay);
        //Default_NormalAttackStartDelay += randomValues[4] * -0.1f;

        artifactStatus.Default_SkillAttackPower += randomValues[5];

        float newDefaultSkillAttackCoolTime = artifactStatus.Default_SkillAttackCoolTime - randomValues[6] * 0.1f;
        artifactStatus.Default_SkillAttackCoolTime = Mathf.Clamp(newDefaultSkillAttackCoolTime, 0, artifactStatus.Default_SkillAttackCoolTime);
        //Default_SkillAttackCoolTime += randomValues[6] * -0.1f;

        artifactStatus.Default_SkillAttackLife += randomValues[7];
        artifactStatus.Default_SkillBulletSpeed += randomValues[8];

        float newSkillAttackStartDelay = artifactStatus.Default_SkillAttackStartDelay - randomValues[9] * 0.1f;
        artifactStatus.Default_SkillAttackStartDelay = Mathf.Clamp(newSkillAttackStartDelay, 0, artifactStatus.Default_SkillAttackStartDelay);
        //Default_SkillAttackStartDelay += randomValues[9] * -0.1f;

        artifactStatus.MaxSlotCount += randomValues[10];
    }

    // Init
    protected void ArtifactInitialize()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
        DevideStatPoint(); // 스탯 포인트 나눠갖기
        SlotRefresh(); // 슬롯 초기화
    }

    // 어택 전 added변수들 초기화
    protected virtual void ResetNormalAttack()
    {
        artifactStatus.ResetNormalAddedStatus();

        PessiveNormalAttack = null; // 기존 패시브 액션 초기화
        BeforeFireNormalAttack = null;
        AfterFireNormalAttack = null; // 기존 발사 후 액션 초기화
    }
    protected virtual void ResetSkillAttack()
    {
        artifactStatus.ResetSkillAddedStatus();

        PessiveSkillAttack = null; // 기존 패시브 액션 초기화
        BeforeFireSkillAttack = null;
        AfterFireSkillAttack = null; // 기존 발사 후 액션 초기화
    }

    // 파츠 추가, 제거
    public void AddParts(ImagePartsRoot_YSJ imageParts, int index)
    {
        //print("파츠 추가: " + imageParts.name);
        GameObject clone = Instantiate(imageParts.gameObject, artifactStatus.SlotTransform);
        clone.transform.SetParent(artifactStatus.SlotTransform.GetChild(index));

        UpdateEnhance();
    }
    public void RemoveParts(int index)
    {
        //print("파츠 제거: " + index);

        Transform targetSlot = artifactStatus.SlotTransform.GetChild(index);

        if (targetSlot.childCount == 0)
        {
            //print("해당 슬롯에 파츠가 없음");
            return;
        }
        else if (targetSlot.childCount > 1)
        {
            //print("해당 슬롯에 파츠가 여러개 있음. 첫번째 파츠만 제거");
        }

        artifactStatus.SlotTransform.GetChild(index).GetChild(0).GetComponent<ImagePartsRoot_YSJ>().WillDestroy = true;
        Destroy(artifactStatus.SlotTransform.GetChild(index).GetChild(0).gameObject);

        UpdateEnhance();
    }

    public void UpdateEnhance()
    {
        //ResetEnhance();
        artifactStatus.ResetPlayerEnhance();

        for (int i = 0; i < artifactStatus.MaxSlotCount; i++)
        {
            ImagePartsRoot_YSJ imagepartsRoot = artifactStatus.SlotTransform.GetChild(i).GetComponentInChildren<ImagePartsRoot_YSJ>();
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
        playerStatus.OffsetMaxHealth = artifactStatus.Added_MaxHealth;
        playerStatus.OffsetMaxMana = artifactStatus.Added_MaxMana;
        playerStatus.OffsetDashCoolTime = artifactStatus.Added_DeahCoolTime;
        playerStatus.OffsetMoveSpeed = artifactStatus.Added_MoveSpeed;
    }
}
