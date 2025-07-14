using UnityEngine;

/// <summary>
/// 스킬 시전 시간 0.05초 증가, 스킬 피해량 20% 증가
/// </summary>
public class ImageParts_Passive_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    //float _delayPercent = 20f;
    float _delayValue = 0.05f;
    float _powerPercent = 20f;

    public override string partsName => "Passive_SkillAttackPower_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        //float addDelay = _delayPercent;
        //addDelay = 0.01f * Mathf.RoundToInt(addDelay);
        float addDelay = _delayValue;
        float addPower = _powerPercent * fireArtifact.skillStatus.Default_AttackPower;
        addPower = Mathf.RoundToInt(addPower) / 100f;

        fireArtifact.skillStatus.Added_AttackStartDelay += addDelay;
        fireArtifact.skillStatus.Added_AttackPower += addPower;
        Debug.Log($"[ckt] {partsName} {fireArtifact.skillStatus.Current_AttackStartDelay}_{addDelay}_{addPower}");
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
