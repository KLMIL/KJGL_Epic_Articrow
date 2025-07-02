using Unity.VisualScripting;
using UnityEngine;

public class ImageParts_HitSkill_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "HitSkill_SkillAttackPower_1";

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
            float add = 0.15f * magicRoot.AttackPower;

            magicRoot.AttackPower += add;
            Debug.Log($"[ckt] {partsName} {add}");
        }
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    #region [스킬 적중 시 플레이어-대상의 거리가 일정 값 이상일 때 추가 스킬 피해]
    float _ThresholdDistance = 5f;
    #endregion
}
