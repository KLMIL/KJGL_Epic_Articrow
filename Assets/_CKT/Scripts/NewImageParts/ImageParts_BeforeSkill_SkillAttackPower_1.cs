using System.Collections;
using UnityEngine;

public class ImageParts_BeforeSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "BeforeSkill_SkillAttackPower_1";

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        //최종 스킬 시전 시간의 30%만큼 스킬 공격 피해 증가
        float add = 0.3f * fireArtifact.Current_SkillAttackStartDelay;

        fireArtifact.Added_SkillAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
}
