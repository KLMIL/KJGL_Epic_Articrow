using UnityEngine;

/// <summary>
/// 이동 속도 증가 퍼센트의 100%만큼 모든 피해량 증가
/// </summary>
public class ImageParts_Passive_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 100f;

    public override string partsName => "Passive_AttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        AttackPower(fireArtifact, spawnedAttack, fireArtifact.normalStatus.Default_AttackPower);
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        AttackPower(fireArtifact, spawnedAttack, fireArtifact.skillStatus.Default_AttackPower);
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion

    #region [상세]
    void AttackPower(Artifact_YSJ fireArtifact, GameObject spawnedAttack, float default_AttackPower)
    {
        YSJ.PlayerStatus playerStatus = fireArtifact.playerStatus;
        float addedPercent = playerStatus.OffsetMoveSpeed / playerStatus.DefaultMoveSpeed;
        addedPercent = 0.01f * Mathf.RoundToInt(addedPercent);
        float add = _increasePercent * addedPercent * default_AttackPower;
        add = 0.01f * Mathf.RoundToInt(add);

        spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower += add;
        Debug.Log($"[ckt] {partsName} {addedPercent}_{add}");
    }
    #endregion
}
