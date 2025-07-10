using UnityEngine;

/// <summary>
/// 일반 공격 쿨타임 0.05초 감소 (최소 쿨타임 0.1초)
/// </summary>
public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    float _percent = 30f;
    float _coolTimeValue = 0.05f;

    public override string partsName => "Passive_NormalAttackCoolTime_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        /*float add = _percent * fireArtifact.normalStatus.Current_AttackCoolTime;
        add = 0.01f * Mathf.RoundToInt(add);

        fireArtifact.normalStatus.Added_AttackCoolTime -= add;
        Debug.Log($"[ckt] {partsName} {add}");*/
        fireArtifact.normalStatus.Added_AttackCoolTime -= _coolTimeValue;
        Debug.Log($"[ckt] {partsName} {_coolTimeValue}");
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
