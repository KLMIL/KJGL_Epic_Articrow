using UnityEngine;

public class ImageParts_Passive_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Passive_AttackPower_1";

    #region [일반 공격]
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

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        YSJ.PlayerStatus playerStatus = BMC.PlayerManager.Instance.PlayerStatus;
        float add = 0.01f * playerStatus.OffsetMoveSpeed;

        fireArtifact.Added_NormalAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion

    #region [스킬 공격]
    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
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
        YSJ.PlayerStatus playerStatus = BMC.PlayerManager.Instance.PlayerStatus;
        float add = 0.01f * playerStatus.OffsetMoveSpeed;

        fireArtifact.Added_SkillAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
    #endregion
}
