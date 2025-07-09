using UnityEngine;

/// <summary>
/// 일반 공격 쿨타임 50% 증가, 일반 공격 적중 시 마나 0.5 획득
/// </summary>
public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    float _increasePercent = 50f;
    int _increaseValue = 1;

    public override string partsName => "Passive_ManaGain_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        float add = _increasePercent * fireArtifact.normalStatus.Default_AttackCoolTime;
        add = 0.01f * Mathf.RoundToInt(add);

        fireArtifact.normalStatus.Added_AttackCoolTime += add;
        Debug.Log($"[ckt] {partsName} AddCoolTime({add})");
    }

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
    #endregion
}
