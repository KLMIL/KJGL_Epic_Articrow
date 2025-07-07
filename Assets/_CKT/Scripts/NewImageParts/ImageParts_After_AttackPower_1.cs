using UnityEngine;

/// <summary>
/// 보호막을 보유하고 있을 때 모든 피해량 30% 증가
/// </summary>
public class ImageParts_After_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 30f;

    public override string partsName => "After_AttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        if (fireArtifact.playerStatus.OffsetBarrier > 0)
        {
            float add = (_increasePercent * 0.01f) * fireArtifact.normalStatus.Default_AttackPower;

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
        if (fireArtifact.playerStatus.OffsetBarrier > 0)
        {
            float add = (_increasePercent * 0.01f) * fireArtifact.skillStatus.Default_AttackPower;

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
    #endregion
}