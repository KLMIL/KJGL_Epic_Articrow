using Game.Enemy;
using UnityEngine;

/// <summary>
/// 스킬 적중 시 대상과의 거리가 4 이상일 때 피해량 20% 증가
/// </summary>
public class ImageParts_HitSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 20f;
    float _ThresholdDistance = 4f;

    public override string partsName => "HitSkill_SkillAttackPower_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

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
        Vector2 dir = (BMC.PlayerManager.Instance.transform.position - hitObject.transform.position);
        float sqrDistance = dir.sqrMagnitude;
        if (sqrDistance >= (_ThresholdDistance * _ThresholdDistance))
        {
            float add = _increasePercent * fireArtifact.skillStatus.Default_AttackPower;
            add = 0.01f * Mathf.RoundToInt(add);

            /*MagicRoot_YSJ magicRoot = spawnedAttack.GetComponent<MagicRoot_YSJ>();
            magicRoot.AttackPower += add;*/
            IDamagable iDamageable = hitObject.GetComponentInChildren<IDamagable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(add);
            }
            Debug.Log($"[ckt] {partsName} {add}");
        }
    }
    #endregion
}
