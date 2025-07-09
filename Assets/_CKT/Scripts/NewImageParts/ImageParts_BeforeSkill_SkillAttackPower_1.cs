using UnityEngine;

/// <summary>
/// 스킬 시전 시간의 1000%만큼 스킬 피해량 증가
/// </summary>
public class ImageParts_BeforeSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 1000f;

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
        float add = _increasePercent * fireArtifact.skillStatus.Current_AttackStartDelay;
        add = 0.01f * Mathf.RoundToInt(add);

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
