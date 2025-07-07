using UnityEngine;

/// <summary>
/// 일반 공격 피해량 20% 증가
/// </summary>
public class ImageParts_Passive_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    float _percent = 20f;

    public override string partsName => "Passive_NormalAttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        float add = (_percent * 0.01f) * fireArtifact.normalStatus.Default_AttackPower;

        fireArtifact.normalStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
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
    }
    #endregion
}