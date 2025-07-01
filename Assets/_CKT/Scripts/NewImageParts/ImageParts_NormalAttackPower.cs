using UnityEngine;

public class ImageParts_NormalAttackPower : ImagePartsRoot_YSJ, IImageParts_YSJ
{
    public override string partsName => "NormalAttackPower";

    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {

    }

    public void AttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {

    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {

    }

    public void Pessive(Artifact_YSJ fireArtifact)
    {
        fireArtifact.Added_NormalAttackPower += 0.15f * fireArtifact.Default_NormalAttackPower;
        Debug.Log($"[ckt] ImageParts_NormalAttackPower {fireArtifact.Added_NormalAttackPower}");
    }
}
