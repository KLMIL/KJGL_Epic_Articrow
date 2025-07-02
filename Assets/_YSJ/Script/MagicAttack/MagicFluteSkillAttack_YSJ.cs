using System.Collections.Generic;
using UnityEngine;

public class MagicFluteSkillAttack_YSJ : MagicRoot_YSJ
{
    float attackDeltaTime;
    int attackIndex;

    private void Start()
    {
        attackDeltaTime = LifeTime / transform.childCount;
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        // AttackDeltatime에 도달하고 공격할 자식이 남아있으면
        if (elapsedTime > attackDeltaTime && attackIndex < transform.childCount) 
        {
            print(attackIndex);
            // 시간기록 초기화
            elapsedTime = 0;
            // 오브젝트 활성화 시켜서 공격
            transform.GetChild(attackIndex).gameObject.SetActive(true);
            // 인덱스 한칸 옮기기
            attackIndex++;
        }
    }

    public override void CountLifeTime(Artifact_YSJ ownerArtifact, GameObject Attack)
    {
        elapsedTime += Time.deltaTime;
    }
}
