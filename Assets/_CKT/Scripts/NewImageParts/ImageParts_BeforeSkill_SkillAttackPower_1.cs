using UnityEngine;

/// <summary>
/// 스킬 시전 시간의 30%만큼 스킬 피해량 증가
/// </summary>
public class ImageParts_BeforeSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 30f;

    public override string partsName => "BeforeSkill_SkillAttackPower_1";

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        float add = (_increasePercent * 0.01f) * fireArtifact.skillStatus.Current_AttackStartDelay;

        fireArtifact.skillStatus.Added_AttackPower += add;
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
