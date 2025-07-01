using UnityEngine;

public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_NormalAttackCoolTime_1";

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
        float add = -0.15f * fireArtifact.Default_NormalAttackCoolTime;

        fireArtifact.Added_NormalAttackCoolTime += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
