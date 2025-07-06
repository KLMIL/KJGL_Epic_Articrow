using UnityEngine;

/// <summary>
/// 일반 공격 쿨타임 20% 증가, 일반 공격 적중 시 마나 0.5 획득
/// </summary>
public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ, IImagePartsToNormalAttack_YSJ
{
    float _increasePercent = 20f;
    int _increaseValue = 1;

    public override string partsName => "Passive_ManaGain_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = (_increasePercent * 0.01f) * currentArtifact.normalStatus.Default_AttackCoolTime;

        currentArtifact.normalStatus.Added_AttackCoolTime += add;
        Debug.Log($"[ckt] {partsName} AddCoolTime({add})");
    }
    #endregion

    #region [Normal]
    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        int add = _increaseValue;

        fireArtifact.playerStatus.RegenerateMana(add);
        Debug.Log($"[ckt] {partsName} RegenerateMana({add})");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion
}
