using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_Light : Artifact_YSJ
{
    private void Start()
    {
        ArtifactInitialize();
    }

    public override IEnumerator NormalAttackCoroutine()
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
                        float firstAngle = angle - ((Current_NormalAttackSpreadAngle * (Current_NormalAttackCount - 1)) / 2f);

                        // 레이저 아티팩트 추가 사항
                        float distance = Current_NormalBulletSpeed; // 원래는 속도 * 시간 = 거리인데 lifeTime이 레이저는 0.1초정도이니 BulletSpeed값을 거리로 씀

                        // 발사 개수만큼 발사
                        for (int SpawnCount = 0; SpawnCount < Current_NormalAttackCount; SpawnCount++)
                        {
                            GameObject SpawnedBullet = Instantiate(NormalAttackPrefab, firePosition.position, Quaternion.Euler(0, 0, firstAngle + Current_NormalAttackSpreadAngle * SpawnCount)); // 첫 각도부터 시작해서 퍼지는 각도 더하면서 탄 생성
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
}
