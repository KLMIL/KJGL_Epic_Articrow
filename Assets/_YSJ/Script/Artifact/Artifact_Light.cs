using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_Light : Artifact_YSJ
{
    public GameObject GuideLine;

    private void Awake()
    {
        BeforeFireSkillAttack += CreateGuideLine;
    }

    private void Start()
    {
        ArtifactInitialize();
    }

    void CreateGuideLine(Artifact_YSJ me)
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; // 방향 각도 계산
        float firstAngle = angle - ((Current_SkillAttackSpreadAngle * (Current_SkillAttackCount - 1)) / 2f);

        // 발사 개수만큼 발사
        for (int SpawnCount = 0; SpawnCount < Current_SkillAttackCount; SpawnCount++)
        {
            //print("spawnGuide");
            GameObject spawnedGuideLine = Instantiate(GuideLine, firePosition.position, Quaternion.Euler(0, 0, firstAngle + Current_SkillAttackSpreadAngle * SpawnCount)); // 첫 각도부터 시작해서 퍼지는 각도 더하면서 탄 생성
            spawnedGuideLine.transform.SetParent(transform, true);
            spawnedGuideLine.GetComponent<MagicGuide_YSJ>().LifeTime = Current_SkillAttackStartDelay;
            spawnedGuideLine.GetComponent<MagicGuide_YSJ>().Speed = Current_SkillBulletSpeed;
        }
    }
}
