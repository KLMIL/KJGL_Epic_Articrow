using UnityEngine;

/// <summary>
/// 추가 마나 1당 스킬 피해량 25% 증가
/// </summary>
public class ImageParts_Passive_SkillAttackPower_3 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    float _percent = 25f;

    public override string partsName => "Passive_SkillAttackPower_3";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = (_percent * 0.01f) * (currentArtifact.playerStatus.OffsetMaxMana * 0.5f);

        currentArtifact.skillStatus.Added_AttackPower += add * currentArtifact.skillStatus.Default_AttackPower;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion
}
