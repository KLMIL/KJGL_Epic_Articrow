using UnityEngine;

/// <summary>
/// 추가 마나 1당 스킬 피해량 30% 증가
/// </summary>
public class ImageParts_Passive_SkillAttackPower_3 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _percent = 30f;

    public override string partsName => "Passive_SkillAttackPower_3";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        float offsetMana = fireArtifact.playerStatus.OffsetMaxMana * 0.5f;
        float add = offsetMana * _percent * fireArtifact.skillStatus.Default_AttackPower;
        add = Mathf.RoundToInt(add) / 100f;

        fireArtifact.skillStatus.Added_AttackPower += add;
        //Debug.Log($"[ckt] {partsName} {add}");
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
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
    #endregion
}
