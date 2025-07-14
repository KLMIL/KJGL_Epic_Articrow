using UnityEngine;

/// <summary>
/// 스킬 시전 시간 0.01초마다 스킬 피해량 1% 증가
/// </summary>
public class ImageParts_BeforeSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent;

    public override string partsName => "BeforeSkill_SkillAttackPower_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        float delay = fireArtifact.skillStatus.Current_AttackStartDelay;
        float add = 100f * delay * fireArtifact.skillStatus.Default_AttackPower;
        add = Mathf.RoundToInt(add) / 100f;

        spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion
}
