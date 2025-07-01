using UnityEngine;

public class ImageParts_Passive_SkillAttackpower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Passive_SkillAttackpower_1";

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
        float addDelay = 0.15f;
        float addPower = 0.15f * fireArtifact.Default_NormalAttackPower;

        fireArtifact.Added_SkillAttackStartDelay += addDelay;
        fireArtifact.Added_SkillAttackPower += addPower;
        Debug.Log($"[ckt] {partsName} {addDelay}_{addPower}");
    }
}
