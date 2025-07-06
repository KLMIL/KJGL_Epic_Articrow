using UnityEngine;

/// <summary>
/// 일반 공격 피해량 15% 증가
/// </summary>
public class ImageParts_Passive_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    float _percent = 15f;

    public override string partsName => "Passive_NormalAttackPower_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = (_percent * 0.01f) * currentArtifact.normalStatus.Default_AttackPower;

        currentArtifact.normalStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion
}