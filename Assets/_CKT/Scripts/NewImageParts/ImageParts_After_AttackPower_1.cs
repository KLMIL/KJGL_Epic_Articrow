using UnityEngine;

public class ImageParts_After_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "After_AttackPower_1";

    #region [Normal]
    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        if (fireArtifact.playerStatus.OffsetBarrier > 0)
        {
            float add = 0.15f * fireArtifact.artifactStatus.Default_NormalAttackPower;

            spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower += add;
            Debug.Log($"[ckt] {partsName} Normal {add}");
        }
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
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        if (fireArtifact.playerStatus.OffsetBarrier > 0)
        {
            float add = 0.15f * fireArtifact.artifactStatus.Default_SkillAttackPower;

            spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower += add;
            Debug.Log($"[ckt] {partsName} Skill {add}");
        }
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
}
