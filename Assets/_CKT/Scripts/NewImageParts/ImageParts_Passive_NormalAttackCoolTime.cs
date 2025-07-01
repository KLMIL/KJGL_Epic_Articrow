using UnityEngine;

public class ImageParts_Passive_NormalAttackCoolTime : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_NormalAttackCoolTime";

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
        throw new System.NotImplementedException();
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        fireArtifact.Added_NormalAttackCoolTime += -0.15f * fireArtifact.Default_NormalAttackCoolTime;

        Debug.Log($"[ckt] Passive_NormalAttackCoolTime {fireArtifact.Added_NormalAttackCoolTime}");
    }
}
