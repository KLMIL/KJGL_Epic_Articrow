using UnityEngine;

/// <summary>
/// 스킬 적중 시 대상과의 거리가 5 이상일 때 피해량 15% 증가
/// </summary>
public class ImageParts_HitSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 15f;
    float _ThresholdDistance = 5f;

    public override string partsName => "HitSkill_SkillAttackPower_1";

    #region [Skill]
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
        Vector2 dir = (BMC.PlayerManager.Instance.transform.position - spawnedAttack.transform.position);
        float sqrDistance = dir.sqrMagnitude;
        if (sqrDistance >= (_ThresholdDistance * _ThresholdDistance))
        {
            MagicRoot_YSJ magicRoot = spawnedAttack.GetComponent<MagicRoot_YSJ>();
            float add = (_increasePercent * 0.01f) * magicRoot.AttackPower;

            magicRoot.AttackPower += add;
            Debug.Log($"[ckt] {partsName} {add}");
        }
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion
}
