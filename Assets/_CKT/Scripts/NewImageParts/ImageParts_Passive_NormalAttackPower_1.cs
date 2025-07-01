using UnityEngine;

public class ImageParts_Passive_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_NormalAttackPower_1";

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
        fireArtifact.Added_NormalAttackPower += 0.15f * fireArtifact.Default_NormalAttackPower;

        Debug.Log($"[ckt] Passive_NormalAttackPower_1 {fireArtifact.Added_NormalAttackPower}");
    }
}
