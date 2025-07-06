using UnityEngine;

/// <summary>
/// 추가 이동 속도의 20%만큼 모든 피해량 증가
/// </summary>
public class ImageParts_Passive_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 20f;

    public override string partsName => "Passive_AttackPower_1";

    #region [Normal]
    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        AttackPower(fireArtifact);
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

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

    #region [Skill]
    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        AttackPower(fireArtifact);
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

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

    #region [상세]
    void AttackPower(Artifact_YSJ fireArtifact)
    {
        YSJ.PlayerStatus playerStatus = fireArtifact.playerStatus;
        float add = (_increasePercent * 0.01f) * playerStatus.OffsetMoveSpeed;

        fireArtifact.normalStatus.Added_AttackPower += add * fireArtifact.normalStatus.Default_AttackPower;
        fireArtifact.skillStatus.Added_AttackPower += add * fireArtifact.normalStatus.Default_AttackPower;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion
}
