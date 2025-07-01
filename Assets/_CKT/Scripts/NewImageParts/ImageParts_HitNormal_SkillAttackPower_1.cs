using UnityEngine;

public class ImageParts_HitNormal_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "HitNormal_SkillAttackPower_1";

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        //TODO : 일반 공격 적중 시 스킬 피해 증가 (스택형, 지속시간)
        //fireArtifact.Added_SkillAttackPower += 0.15f * fireArtifact.Default_SkillAttackPower;

        Debug.Log($"[ckt] HitNormal_SkillAttackPower_1 {""}");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
}
