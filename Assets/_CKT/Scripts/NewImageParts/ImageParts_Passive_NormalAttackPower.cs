using UnityEngine;

public class ImageParts_Passive_NormalAttackPower : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_NormalAttackPower";

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
        fireArtifact.Added_NormalAttackPower += 0.15f * fireArtifact.Default_NormalAttackPower;

        Debug.Log($"[ckt] Passive_NormalAttackPower {fireArtifact.Added_NormalAttackPower}");
    }
}
