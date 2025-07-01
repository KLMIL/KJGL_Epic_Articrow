using UnityEngine;

public class ImageParts_NormalAttackCooldown : ImagePartsRoot_YSJ, IImageParts_YSJ
{
    public override string partsName => "NormalAttackCooldown";

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
        fireArtifact.Added_NormalAttackCoolTime += -0.15f * fireArtifact.Default_NormalAttackCoolTime;
        Debug.Log($"[ckt] ImageParts_NormalAttackCooldown {fireArtifact.Added_NormalAttackCoolTime}");
    }
}
