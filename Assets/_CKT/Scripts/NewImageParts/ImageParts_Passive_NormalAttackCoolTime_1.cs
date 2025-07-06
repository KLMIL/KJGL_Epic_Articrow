using UnityEngine;

/// <summary>
/// 일반 공격 쿨타임 15% 감소
/// </summary>
public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    float _percent = 15f;

    public override string partsName => "Passive_NormalAttackCoolTime_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = (_percent * 0.01f) * currentArtifact.normalStatus.Default_AttackCoolTime;

        currentArtifact.normalStatus.Added_AttackCoolTime -= add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion
}
