using UnityEngine;

/// <summary>
/// 스킬 시전 시간 20% 증가, 스킬 피해량 20% 증가
/// </summary>
public class ImageParts_Passive_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    float _delayPercent = 20f;
    float _powerPercent = 20f;

    public override string partsName => "Passive_SkillAttackPower_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        float addDelay = (_delayPercent * 0.01f);
        float addPower = (_powerPercent * 0.01f) * currentArtifact.normalStatus.Default_AttackPower;

        currentArtifact.skillStatus.Added_AttackStartDelay += addDelay;
        currentArtifact.skillStatus.Added_AttackPower += addPower;
        Debug.Log($"[ckt] {partsName} {addDelay}_{addPower}");
    }
    #endregion
}
