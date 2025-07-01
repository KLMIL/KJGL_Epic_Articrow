using UnityEngine;

public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_NormalAttackCoolTime_1";

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
        fireArtifact.Added_NormalAttackCoolTime += -0.15f * fireArtifact.Default_NormalAttackCoolTime;

        Debug.Log($"[ckt] Passive_NormalAttackCoolTime_1 {fireArtifact.Added_NormalAttackCoolTime}");
    }
}
