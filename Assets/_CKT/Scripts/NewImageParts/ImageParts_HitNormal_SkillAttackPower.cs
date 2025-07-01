using UnityEngine;

public class ImageParts_HitNormal_SkillAttackPower : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "HitNormal_SkillAttackPower";

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        //fireArtifact.Added_SkillAttackPower += 0.15f * fireArtifact.Default_SkillAttackPower;

        Debug.Log($"[ckt] HitNormal_SkillAttackPower {""}");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        throw new System.NotImplementedException();
    }
}
