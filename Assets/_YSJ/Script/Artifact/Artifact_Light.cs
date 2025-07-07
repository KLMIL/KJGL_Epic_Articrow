using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_Light : Artifact_YSJ
{
    public GameObject GuideLineObject;

    private void Awake()
    {
    }

    private void Start()
    {
        ArtifactInitialize();
    }

    protected override void ResetNormalAttack()
    {
        base.ResetNormalAttack();
        normalStatus.AfterFire += PlayNormalSFX; // 일반 공격 생성 한 직후 사운드 재생
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.BeforeFire += CreateGuideLine;
    }

    void PlayNormalSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.LightNormalAttack);
    }

    void CreateGuideLine(Artifact_YSJ me) 
    {
        StartCoroutine(CreateGuide(GuideLineObject));
    }

    // 가이드 라인이 공격인 애들
    IEnumerator CreateGuide(GameObject GuideObject)
    {
        // 공격 생성
        if (GuideObject)
        {
            // 추가 발사 개수만큼 반복
            for (int addedSpawnCount = 0; addedSpawnCount < skillStatus.Added_AttackCount; addedSpawnCount++)
            {
                // 디폴트 발사 개수만큼 반복
                for (int SpawnCount = 0; SpawnCount < skillStatus.Default_AttackCount; SpawnCount++)
                {
                    // 발사하기 전에 방향 다시계산
                    Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
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

                            GameObject SpawnedBullet = Instantiate(GuideObject, firePosition.position, Quaternion.Euler(0, 0, defaultSpreadAngle)); // 각도에 맞게 탄 생성

                            SpawnedBullet.transform.localScale = Vector3.one * skillStatus.Current_AttackScale; // 공격 크기 설정
                            SpawnedBullet.GetComponent<MagicRoot_YSJ>().SkillAttackInitialize(this); // 마법에 정보 주입(데미지, 스피드, 탄 지속시간 등)

                            // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
                            ReadSkillAttackParts(SpawnedBullet.GetComponent<MagicRoot_YSJ>());

                            // 일반 공격 생성 한 직후 액션 실행
                            skillStatus.AfterFire?.Invoke(this, SpawnedBullet);
                        }
                    }
                    // 마지막 공격이 아니라면 공격 간격만큼 기다리기
                    if (SpawnCount + 1 < skillStatus.Default_AttackCount)
                    {
                        yield return new WaitForSeconds(skillStatus.Default_AttackCountDeltaTime);
                    }
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
        else
        {
            print("가이드 프리팹 설정안됌!");
        }
    }
}
